using Acr.UserDialogs;
using Fusillade;
using MonkeyCache.SQLite;
using Newtonsoft.Json;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UndderControlLib.Dtos;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UndderControl.Services
{
    public class ApiManager : IApiManager
    {
        private readonly IUserDialogs _userDialogs = UserDialogs.Instance;
        private Dictionary<int, CancellationTokenSource> _runningTasks = new Dictionary<int, CancellationTokenSource>();
        private readonly IApiService<IFarmApi> _farmApi;
        private readonly IApiService<IFarmUserApi> _farmUserApi;
        private readonly IApiService<IFarmTypeApi> _farmTypeApi;
        private readonly IApiService<ISurveyApi> _surveyApi;
        private readonly IApiService<ICowStatusApi> _cowStatusApi;
        private readonly IApiService<ICowStatusFarmApi> _cowStatusFarmApi;
        private readonly IApiService<ISurveyResponseApi> _surveyResponseApi;
        private readonly IApiService<IUserApi> _userApi;
        private readonly double _cacheExpiryDays = Config.MonkeyCacheExpiry;
        private readonly IMetricsManagerService _metricsManager;
        public bool IsConnected { get; set; }

        public ApiManager(IApiService<IFarmApi> farmApi, IApiService<IFarmUserApi> farmUserApi, IApiService<IFarmTypeApi> farmTypeApi, IApiService<ISurveyApi> surveyApi, IApiService<ICowStatusApi> cowStatusApi, IApiService<ICowStatusFarmApi> cowStatusFarmApi, IApiService<ISurveyResponseApi> surveyResponseApi, IApiService<IUserApi> userApi, IMetricsManagerService metricsManager)
        {
            IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            _farmApi = farmApi;
            _farmUserApi = farmUserApi;
            _farmTypeApi = farmTypeApi;
            _surveyApi = surveyApi;
            _cowStatusApi = cowStatusApi;
            _cowStatusFarmApi = cowStatusFarmApi;
            _surveyResponseApi = surveyResponseApi;
            _userApi = userApi;
            _metricsManager = metricsManager;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;

            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                //Cancel all running tasks
                try
                {
                    foreach (var item in _runningTasks)
                    {
                        item.Value.Cancel();
                        _runningTasks.Remove(item.Key);
                    }
                }
                catch(Exception ex)
                {
                    _metricsManager.TrackException(ex.Message, ex);
                }
            }
        }

        protected async Task<TData> RemoteRequestAsync<TData>(Task<TData> task, string barrel)
            where TData : HttpResponseMessage,
            new()
        {
            TData data = new TData();

            // If no network try and pull data from cache or default to error
            if (!IsConnected)
            {
                if (!string.IsNullOrEmpty(barrel) && !Barrel.Current.IsExpired(barrel) && Barrel.Current.Exists(barrel))
                {
                    data.StatusCode = HttpStatusCode.NotModified;
                    data.Content = new StringContent(Barrel.Current.Get<string>(barrel));
                    return data;
                }

                var toastResponse = "There is no network connection";
                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(toastResponse);
                _userDialogs.Toast(toastResponse, TimeSpan.FromSeconds(1));
                return data;
            }

            // Check for fresh cached copy
            if (!string.IsNullOrEmpty(barrel) && Barrel.Current.Exists(barrel) && !Barrel.Current.IsExpired(barrel))
            {
                data.StatusCode = HttpStatusCode.NotModified;
                data.Content = new StringContent(Barrel.Current.Get<string>(barrel));
                return data;
            }

            data = await Policy
                .Handle<WebException>()
                .Or<ApiException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(
                    retryCount: 2,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () =>
                {
                    var result = await task;
                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        // Logout code
                    }
                    return result;

                });
            // If we make it this far and we have a monkeycache barrel id update our local cache and reset expiry
            if (data.IsSuccessStatusCode && !string.IsNullOrEmpty(barrel))
            {
                try
                {
                    var content = await data.Content.ReadAsStringAsync();
                    Barrel.Current.Add(barrel, content, TimeSpan.FromDays(_cacheExpiryDays));
                } catch (Exception ex)
                {
                   // _metricsManager.TrackException(ex.Message, ex);
                }
            }

            return data;
        }

        public async Task<HttpResponseMessage> FarmList()
        {
                var cts = new CancellationTokenSource();
                var task = RemoteRequestAsync(_farmApi.GetApi(Priority.UserInitiated).FarmList(), "FarmList");
                _runningTasks.Add(task.Id, cts);

                return await task;            
        }

        public async Task<HttpResponseMessage> UploadResponse(SurveyResponseDto survey)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_surveyResponseApi.GetApi(Priority.UserInitiated).UploadSurvey(survey), string.Empty);
            _runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetLatestSurvey()
        {
            if (Config.TestMode)
            {
                string fileContents;
                using (var stream = await FileSystem.OpenAppPackageFileAsync("Survey.txt"))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        fileContents = await reader.ReadToEndAsync();
                    }
                }

                var response = new HttpResponseMessage
                {
                    Content = new StringContent(fileContents),
                    StatusCode = HttpStatusCode.OK
                };

                return response;
            }
            else
            {
                var cts = new CancellationTokenSource();
                var task = RemoteRequestAsync(_surveyApi.GetApi(Priority.UserInitiated).GetLatestSurvey(), null);
                _runningTasks.Add(task.Id, cts);

                return await task;
        }
    }

        public async Task<HttpResponseMessage> UploadFarm(FarmDto farm, bool isNew)
        {
            if (isNew)
            {
                var cts = new CancellationTokenSource();
                var task = RemoteRequestAsync(_farmApi.GetApi(Priority.UserInitiated).CreateFarm(farm), null);
                _runningTasks.Add(task.Id, cts);

                return await task;
            }
            else
            {
                var cts = new CancellationTokenSource();
                var task = RemoteRequestAsync(_farmApi.GetApi(Priority.UserInitiated).UpdateFarm(farm), null);
                _runningTasks.Add(task.Id, cts);

                return await task;
            }
        }

        public async Task<HttpResponseMessage> CreateCowStatus(CowStatusDto status)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_cowStatusApi.GetApi(Priority.UserInitiated).CreateCowStatus(status), null);
            _runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetFarmsByUserId(int id)
        {
            if (Config.TestMode)
            {
                string fileContents = string.Empty;
                try
                {
                    using (var stream = await FileSystem.OpenAppPackageFileAsync("Farms.txt"))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            fileContents = await reader.ReadToEndAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _metricsManager.TrackException("Error retrieving farm details from embedded resource", ex);
                }

                var response = new HttpResponseMessage
                {
                    Content = new StringContent(fileContents),
                    StatusCode = HttpStatusCode.OK
                };

                return response;
            }
            else
            {
                try
                {
                    var cts = new CancellationTokenSource();
                    var task = RemoteRequestAsync(_farmUserApi.GetApi(Priority.UserInitiated).GetFarmsByUserId(id), "GetFarmsByUserId" + id);
                    _runningTasks.Add(task.Id, cts);

                    return await task;
                }
                catch (Exception ex)
                {
                    _metricsManager.TrackException(ex.Message, ex);
                    return null;
                }
                
            }
        }

        public async Task<HttpResponseMessage> GetResponseByFarmId(int id)
        {
            if (Config.TestMode)
            {
                string fileContents = string.Empty;
                try
                {
                    using (var stream = await FileSystem.OpenAppPackageFileAsync("SurveyResponse.txt"))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            fileContents = await reader.ReadToEndAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _metricsManager.TrackException("Error retrieving farm details from embedded resource", ex);
                }

                var response = new HttpResponseMessage
                {
                    Content = new StringContent(fileContents),
                    StatusCode = HttpStatusCode.OK
                };

                return response;
            }
            else
            {
                try
                {
                    var cts = new CancellationTokenSource();
                    var task = RemoteRequestAsync(_surveyResponseApi.GetApi(Priority.UserInitiated).GetResponseByFarmId(id), "GetResponseByFarmId" + id);
                    _runningTasks.Add(task.Id, cts);

                    return await task;
                }
                catch (Exception ex)
                {
                    _metricsManager.TrackException(ex.Message, ex);
                    return null;
                }
            }
        }

        public async Task<HttpResponseMessage> GetStatusByFarmId(int id)
        {
            if (Config.TestMode)
            {
                string fileContents = string.Empty;
                try
                {
                    using (var stream = await FileSystem.OpenAppPackageFileAsync("CowStatus19.txt"))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            fileContents = await reader.ReadToEndAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _metricsManager.TrackException("Error retrieving cow status 19 from embedded resource", ex);
                }

                var response = new HttpResponseMessage
                {
                    Content = new StringContent(fileContents),
                    StatusCode = HttpStatusCode.OK
                };

                return response;

            }
            else
            {
                var cts = new CancellationTokenSource();
                var task = RemoteRequestAsync(_cowStatusApi.GetApi(Priority.UserInitiated).GetStatusByFarmId(id), "GetStatusByFarmId"+id);
                _runningTasks.Add(task.Id, cts);

                return await task;
            }
        }

        public async Task<HttpResponseMessage> GetUserByToken(string token)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_userApi.GetApi(Priority.UserInitiated).GetUserByToken(token), "GetUserById" + token);
            _runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> CreateUser(UserDto user)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_userApi.GetApi(Priority.UserInitiated).CreateUser(user), null);
            _runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> UpdateUser(UserDto user)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_userApi.GetApi(Priority.UserInitiated).UpdateUser(user), null);
            _runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetCowsStatusByFarmID(int id)
        {
            if (Config.TestMode)
            {
                string fileContents = string.Empty;
                try
                {
                    using (var stream = await FileSystem.OpenAppPackageFileAsync("CowStatus19.txt"))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            fileContents = await reader.ReadToEndAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _metricsManager.TrackException("Error retrieving cow status 19 from embedded resource", ex);
                }

                var response = new HttpResponseMessage
                {
                    Content = new StringContent(fileContents),
                    StatusCode = HttpStatusCode.OK
                };

                return response;

            }
            else
            {
                try
                {
                    var cts = new CancellationTokenSource();
                    var task = RemoteRequestAsync(_cowStatusFarmApi.GetApi(Priority.UserInitiated).GetCowsStatusByFarmID(id), null);
                    _runningTasks.Add(task.Id, cts);

                    return await task;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
        }

        public async Task<HttpResponseMessage> GetCowsStatusByFarmIDandYear(int id, int year)
        {
            if (Config.TestMode)
            {
                string fileContents = string.Empty;
                try
                {
                    using (var stream = await FileSystem.OpenAppPackageFileAsync("CowStatus18.txt"))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            fileContents = await reader.ReadToEndAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _metricsManager.TrackException("Error retrieving cow status 2018 from embedded resource", ex);
                }

                var response = new HttpResponseMessage
                {
                    Content = new StringContent(fileContents),
                    StatusCode = HttpStatusCode.OK
                };

                return response;

            }
            else
            {
                var cts = new CancellationTokenSource();
                var task = RemoteRequestAsync(_cowStatusFarmApi.GetApi(Priority.UserInitiated).GetCowsStatusByFarmIDandYear(id, year), null);
                _runningTasks.Add(task.Id, cts);

                return await task;
            }
        }

        public async Task<HttpResponseMessage> GetFarmById(int id)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_farmApi.GetApi(Priority.UserInitiated).GetFarmById(id), null);
            _runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> GetFarmTypes()
        {
            if (Config.TestMode)
            {
                string fileContents = string.Empty;
                try
                {
                    using (var stream = await FileSystem.OpenAppPackageFileAsync("FarmType.txt"))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            fileContents = await reader.ReadToEndAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _metricsManager.TrackException("Error retrieving farm types from embedded resource", ex);
                }

                var response = new HttpResponseMessage
                {
                    Content = new StringContent(fileContents),
                    StatusCode = HttpStatusCode.OK
                };

                return response;

            }
            else
            {
                var cts = new CancellationTokenSource();
                var task = RemoteRequestAsync(_farmTypeApi.GetApi(Priority.UserInitiated).GetFarmTypes(), "GetFarmTypes");
                _runningTasks.Add(task.Id, cts);

                return await task;
            }
        }

        public async Task<HttpResponseMessage> UpdateCowStatus(CowStatusDto status)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_cowStatusApi.GetApi(Priority.UserInitiated).UpdateCowStatus(status), null);
            _runningTasks.Add(task.Id, cts);

            return await task;
        }
    }
}
