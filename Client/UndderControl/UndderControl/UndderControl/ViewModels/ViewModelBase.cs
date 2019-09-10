using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UndderControl.Services;
using Xamarin.Forms;

namespace UndderControl.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        protected IMetricsManagerService MetricsManager { get; private set; }
        private bool _isBusy;
        private string _title;
        /// <summary>
        /// If <code>true</code>, indicates the view is busy.
        /// </summary>
        /// <remarks>
        /// This is a useful property to bind an activity indicator visibility to.
        /// </remarks>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    RaisePropertyChanged(nameof(IsNotBusy));
                }
            }
        }
        /// <summary>
        /// If <code>true</code>, signals the activity is not busy.
        /// </summary>
        /// <remarks>
        /// This is a useful property to bind a button enabled property to.
        /// </remarks>
        public bool IsNotBusy => !_isBusy;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public IUserDialogs PageDialog = UserDialogs.Instance;
        public IApiManager ApiManager;
        readonly IApiService<IFarmApi> farmApi = new ApiService<IFarmApi>(Config.ApiUrl);
        readonly IApiService<ISurveyApi> surveyApi = new ApiService<ISurveyApi>(Config.ApiUrl);
        readonly IApiService<ICowStatusApi> cowStatusApi = new ApiService<ICowStatusApi>(Config.ApiUrl);
        readonly IApiService<ISurveyResponseApi> surveyResponseApi = new ApiService<ISurveyResponseApi>(Config.ApiUrl);
        readonly IApiService<IUserApi> userApi = new ApiService<IUserApi>(Config.ApiUrl);

        public ViewModelBase(INavigationService navigationService, IMetricsManagerService metricsManager)
        {
            MetricsManager = metricsManager;
            NavigationService = navigationService;
            ApiManager = new ApiManager(farmApi, surveyApi, cowStatusApi, surveyResponseApi, userApi, MetricsManager);
        }

        public async Task RunSafe(Task task, bool ShowLoading = true, string loadingMessage = null)
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                if (ShowLoading) UserDialogs.Instance.ShowLoading(loadingMessage ?? "Loading");
                await task;
            }
            catch (Exception e)
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IMetricsManagerService>().TrackException("TaskRunSafeException", e);
            }
            finally
            {
                IsBusy = false;
                if (ShowLoading) UserDialogs.Instance.HideLoading();
            }
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
