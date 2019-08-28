﻿using Newtonsoft.Json;
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
        private readonly INavigationService _navigationService;
        private readonly IMetricsManagerService _metricsService;
        private DelegateCommand<string> _navigateCommand;

        public DelegateCommand<string> OnNavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(NavigateAsync));

        public AssessmentPageViewModel(INavigationService navigationService, IMetricsManagerService metricsService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _metricsService = metricsService;
            Title = "Undder Control";
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

        private async void NavigateAsync(string page)
        {
            _metricsService.TrackEvent("Navigate: " + page);
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }
    }
}
