using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UndderControl.Extensions;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControlLib.Dtos;
using Xamarin.Forms;

namespace UndderControl.ViewModels
{
    public class AssessmentPageViewModel : ViewModelBase
    {
        private DelegateCommand<string> _navigateCommand;

        public DelegateCommand<string> OnNavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(NavigateAsync));

        private DelegateCommand<string> _onSummaryCommand;
        public DelegateCommand<string> OnSummaryCommand
            => _onSummaryCommand ?? (_onSummaryCommand = new DelegateCommand<string>(NavigateAsync, CanSummaryNavigate));

        private DelegateCommand<string> _onCompareCommand;
        public DelegateCommand<string> OnCompareCommand
            => _onCompareCommand ?? (_onCompareCommand = new DelegateCommand<string>(NavigateAsync, CanCompareNavigate));

        public AssessmentPageViewModel(INavigationService navigationService, IMetricsManagerService metricsService)
            : base(navigationService, metricsService)
        {
            Title = "UnDDER CONTROL";
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
                // Pull existing responses from the database
                await RunSafe(GetData());
                
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException("GetAssessmentDataFailed", ex);
            }
        }

        async Task GetData()
        {
            var surveyResponse = await ApiManager.GetLatestSurvey();
            if (surveyResponse.IsSuccessStatusCode)
            {
                try
                {
                    var content = await surveyResponse.Content.ReadAsStringAsync();
                    var survey = await Task.Run(() => JsonConvert.DeserializeObject<SurveyDto>(content));
                    if (survey != null && (App.LatestSurvey == null || App.LatestSurvey.Version < survey.Version))
                    {
                        var fileHelper = new FileHelper();
                        App.LatestSurvey = survey;
                    }
                }
                catch (Exception ex)
                {
                    MetricsManager.TrackException("Error reading survey json", ex);
                }

            }
            else
            {
                await PageDialog.AlertAsync("Unable to retrieve survey data", "Error", "OK");
            }

            var serviceResponse = await ApiManager.GetResponseByFarmId(App.SelectedFarm.ID);
            if (serviceResponse.IsSuccessStatusCode || serviceResponse.StatusCode == HttpStatusCode.NotModified)
            {
                try
                {
                    var response = await serviceResponse.Content.ReadAsStringAsync();
                    var responseData = await Task.Run(() => JsonConvert.DeserializeObject<List<SurveyResponseDto>>(response));
                    if (responseData != null && responseData.Count == 1)
                    {
                        App.LatestSurveyResponse = responseData[0];
                        OnSummaryCommand.RaiseCanExecuteChanged();
                    }
                    else if (responseData != null && responseData.Count == 2)
                    {
                        App.LatestSurveyResponse = responseData[0];
                        App.PreviousSurveyResponse = responseData[1];
                        OnSummaryCommand.RaiseCanExecuteChanged();
                        OnCompareCommand.RaiseCanExecuteChanged();
                    }
                }
                catch (Exception ex)
                {
                    MetricsManager.TrackException("Error reading survey json", ex);
                }
            }
        }

        private async void NavigateAsync(string page)
        {
            MetricsManager.TrackEvent("Navigate: " + page);
            await NavigationService.NavigateAsync(page);
        }

        private bool CanSummaryNavigate(string arg)
        {
            return App.LatestSurveyResponse == null ? false : true;
        }
        private bool CanCompareNavigate(string arg)
        {
            return (App.LatestSurveyResponse == null ? false : true) && (App.PreviousSurveyResponse == null ? false : true);
        }
    }
}
