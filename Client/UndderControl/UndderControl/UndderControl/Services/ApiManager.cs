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
        private readonly IApiService<ISurveyApi> _surveyApi;
        private readonly IApiService<ICowStatusApi> _cowStatusApi;
        private readonly double _cacheExpiryDays = Config.MonkeyCacheExpiry;
        public bool IsConnected { get; set; }

        public ApiManager(IApiService<IFarmApi> farmApi, IApiService<ISurveyApi> surveyApi, IApiService<ICowStatusApi> cowStatusApi)
        {
            IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            _farmApi = farmApi;
            _surveyApi = surveyApi;
            _cowStatusApi = cowStatusApi;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;

            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                //Cancel all running tasks
                //TODO: Need a queue system here to add these failed tasks
                foreach(var item in _runningTasks)
                {
                    item.Value.Cancel();
                    _runningTasks.Remove(item.Key);
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

            // Check for fresh cached copy of farms
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

            // If we make it this far and we have a monkey cache barrel id update our local cache and reset expiry
            if (data.IsSuccessStatusCode && !string.IsNullOrEmpty(barrel))
            {
                Barrel.Current.Add(barrel, data.Content.ToString(), TimeSpan.FromDays(_cacheExpiryDays));
            }

            return data;
        }

        public async Task<HttpResponseMessage> FarmList()
        {
            //if (Config.TestMode)
            //{
            //    string fileContents = string.Empty;
            //    try
            //    {
            //        using (var stream = await FileSystem.OpenAppPackageFileAsync("Farms.txt"))
            //        {
            //            using (var reader = new StreamReader(stream))
            //            {
            //                fileContents = await reader.ReadToEndAsync();
            //            }
            //        }                    
            //    }
            //    catch (Exception ex)
            //    {
            //        DependencyService.Get<IMetricsManagerService>().TrackException("Error retrieving farm details from embedded resource", ex);
            //    }

            //    var response = new HttpResponseMessage
            //    {
            //        Content = new StringContent(fileContents),
            //        StatusCode = HttpStatusCode.OK
            //    };

            //    return response;

            //}
            //else
            //{
                var cts = new CancellationTokenSource();
                var task = RemoteRequestAsync(_farmApi.GetApi(Priority.UserInitiated).FarmList(), "FarmList");
                _runningTasks.Add(task.Id, cts);

                return await task;
            //}
            
        }

        public async Task<HttpResponseMessage> UploadSurvey(SurveyResponseDto survey)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync(_surveyApi.GetApi(Priority.UserInitiated).UploadSurvey(survey), string.Empty);
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

        public async Task<HttpResponseMessage> UploadCowStatus(CowStatusDto status)
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
                    DependencyService.Get<IMetricsManagerService>().TrackException("Error retrieving farm details from embedded resource", ex);
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
            var task = RemoteRequestAsync(_farmApi.GetApi(Priority.UserInitiated).GetFarmsByUserId(id), "GetFarmsByUserId");
            _runningTasks.Add(task.Id, cts);

            return await task;
        }

    }
    }
}
