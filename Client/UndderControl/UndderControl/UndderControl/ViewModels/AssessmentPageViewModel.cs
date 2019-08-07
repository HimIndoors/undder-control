using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Extensions;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControl.Views;
using UndderControlLib.Dtos;
using Xamarin.Forms;

namespace UndderControl.ViewModels
{
    public class AssessmentPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;      

        public DelegateCommand StartSurveyCommand { get; private set; }
        public DelegateCommand ShowSummaryCommand { get; private set; }
        public DelegateCommand ShowCompareCommand { get; private set; }

        public AssessmentPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Undder Control";
            StartSurveyCommand = new DelegateCommand(StartSurvey);
            ShowSummaryCommand = new DelegateCommand(ShowSummary);
            ShowCompareCommand = new DelegateCommand(ShowCompare);
            IsBusy = true;
            InitAsync();
        }

        private async void InitAsync()
        {
            string surveyFileName = Config.SurveyFileName;
            // Check if the survey has already been downloaded
            var fileHelper = new FileHelper();
            if (await fileHelper.ExistsAsync(surveyFileName))
            {
                // Load the survey if it exists
                App.LatestSurvey = await fileHelper.LoadAsync<SurveyDto>(surveyFileName);
            }

            try
            {
                // Check if an updated survey is available from the service
                await RunSafe(GetSurvey());
                
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMetricsManagerService>().TrackException("GetSurveyFailed", ex);
            }

            IsBusy = false;
        }

        async Task GetSurvey()
        {
            var response = await ApiManager.GetLatestSurvey();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var survey = await Task.Run(() => JsonConvert.DeserializeObject<SurveyDto>(content));
                    if (survey != null && (App.LatestSurvey == null || App.LatestSurvey.Version < survey.Version))
                    {
                        var fileHelper = new FileHelper();
                        App.LatestSurvey = survey;
                        // Save the survey to the local filesystem
                        await fileHelper.SaveAsync(Config.SurveyFileName, survey);
                    }
                }
                catch (Exception ex)
                {
                    DependencyService.Get<IMetricsManagerService>().TrackException("Error reading survey json", ex);
                }
                
            }
            else
            {
                await PageDialog.AlertAsync("Unable to retrieve survey data", "Error", "OK");
            }
        }
        private async void StartSurvey()
        {
            DependencyService.Get<IMetricsManagerService>().TrackEvent("StartSurvey");
            await _navigationService.NavigateAsync("SdctMasterDetailPage/NavigationPage/SurveyPage");
        }

        private async void ShowSummary()
        {
            DependencyService.Get<IMetricsManagerService>().TrackEvent("ShowSummary");
            await _navigationService.NavigateAsync("SdctMasterDetailPage/NavigationPage/ResultsPage");
        }

        private async void ShowCompare()
        {
            DependencyService.Get<IMetricsManagerService>().TrackEvent("ShowCompare");
            await _navigationService.NavigateAsync("SdctMasterDetailPage/NavigationPage/ComparePage");
        }

    }
}
