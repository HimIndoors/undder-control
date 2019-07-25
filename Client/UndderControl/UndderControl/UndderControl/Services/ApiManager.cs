using Acr.UserDialogs;
using Fusillade;
using MonkeyCache.SQLite;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UndderControlLib.Dtos;
using Xamarin.Essentials;

namespace UndderControl.Services
{
    public class ApiManager : IApiManager
    {
        private IUserDialogs _userDialogs = UserDialogs.Instance;
        private Dictionary<int, CancellationTokenSource> _runningTasks = new Dictionary<int, CancellationTokenSource>();
        private Dictionary<string, Task<HttpResponseMessage>> _taskContainer = new Dictionary<string, Task<HttpResponseMessage>>();
        private IApiService<IFarmApi> _farmApi;
        private IApiService<ISurveyApi> _surveyApi;
        private double _cacheExpiryDays = Config.MonkeyCacheExpiry;
        public bool IsConnected { get; set; }

        public ApiManager(IApiService<IFarmApi> farmApi, IApiService<ISurveyApi> surveyApi)
        {
            IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            _farmApi = farmApi;
            _surveyApi = surveyApi;
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

            // If no network try and pull data from cache then default to error
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
            if (!string.IsNullOrEmpty(barrel) && !Barrel.Current.IsExpired(barrel) && Barrel.Current.Exists(barrel))
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
                    retryCount: 5,
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
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(_farmApi.GetApi(Priority.UserInitiated).FarmList(), Config.MonkeyCacheFarms);
            _runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> UploadSurvey(SurveyResponseDto survey)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(_surveyApi.GetApi(Priority.UserInitiated).UploadSurvey(survey), string.Empty);
            _runningTasks.Add(task.Id, cts);

            return await task;
        }
    }
}
