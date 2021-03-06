﻿using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UndderControl.Events;
using UndderControl.Extensions;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class RootPageViewModel : ViewModelBase
    {
        private ObservableCollection<FarmDto> _farmList;
        public ObservableCollection<FarmDto> FarmList
        {
            get { return _farmList; }
            set
            {
                _farmList = value;
                RaisePropertyChanged();
            }
        }
        private FarmDto _selectedFarm { get; set; }
        public FarmDto SelectedFarm
        {
            get { return _selectedFarm; }
            set
            {
                _selectedFarm  = value;
                if (App.SelectedFarm == null)
                    App.SelectedFarm = value; //Update global farm
                FrameEnabled = true;
                FrameTextColour = "#009096";
                EditFarmEnabled = true;
                OnNavigateCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedFarm");
            }
        }
        private string _frameTextColour;
        public string FrameTextColour
        {
            get { return _frameTextColour; }
            set {
                _frameTextColour = value;
                RaisePropertyChanged("FrameTextColour");
            }
        }
        private bool _frameEnabled;
        public bool FrameEnabled
        {
            get { return _frameEnabled; }
            set {
                _frameEnabled = value;
                RaisePropertyChanged("FrameEnabled");
            }
        }
        private string _frameAssessmentColour;
        public string FrameAssessmentColour
        {
            get { return _frameAssessmentColour; }
            set {
                _frameAssessmentColour = value;
                RaisePropertyChanged("FrameAssessmentColour");
            }
        }
        private string _frameMonitorColour;
        public string FrameMonitorColour
        {
            get { return _frameMonitorColour; }
            set {
                _frameMonitorColour = value;
                RaisePropertyChanged("FrameMonitorColour");
            }
        }
        private bool _editFarmEnabled;
        public bool EditFarmEnabled
        {
            get { return _editFarmEnabled; }
            set
            {
                _editFarmEnabled = value;
                RaisePropertyChanged("EditFarmEnabled");
            }
        }

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> OnNavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(NavigateAsync, CanNavigate));

        private readonly IPageDialogService _dialogService;

        private DelegateCommand _onEditFarmCommand;
        public DelegateCommand OnEditFarmCommand =>
            _onEditFarmCommand ?? (_onEditFarmCommand = new DelegateCommand(EditFarm));

        private readonly IEventAggregator _eventAggregator;

        public RootPageViewModel(INavigationService navigationService, IMetricsManagerService metricsSevice, IPageDialogService dialogueService, IEventAggregator eventAggregator) 
            : base(navigationService, metricsSevice)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogueService;
            Title = "UnDDER CONTROL";
            InitAsync();
        }

        async void InitAsync()
        {
            FrameEnabled = false;
            FrameTextColour = "#cccccc";
            try
            {
                await RunSafe(GetFarms());

                //New user with no farms so direct to add farm page
                if (FarmList.Count > 1)
                {
                    var result = await _dialogService.DisplayAlertAsync("No farms found", "Would you like to add a farm?", "Yes", "No");
                    if (result)
                    {
                        await NavigationService.NavigateAsync("ManageFarmsPage");
                    }
                }

                //If we havent already selected a farm and only a single farm found pre select it
                if (App.SelectedFarm == null && FarmList.Count == 1)
                    App.SelectedFarm = FarmList[0];

                //Set up screen based on selected farm
                if (App.SelectedFarm != null)
                {
                    EditFarmEnabled = true;
                    SelectedFarm = App.SelectedFarm;
                    _eventAggregator.GetEvent<RootPageRefreshEvent>().Publish();
                }
                else
                {
                    EditFarmEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException("GetFarmsFailed", ex);
            }

            //Load latest survey here
            try
            {
                await RunSafe(GetSurvey());
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException("GetFarmsFailed", ex);
            }
        }

        async Task GetSurvey()
        {
            var surveyResponse = await ApiManager.GetLatestSurvey();
            if (surveyResponse.IsSuccessStatusCode || surveyResponse.StatusCode == HttpStatusCode.NotModified)
            {
                try
                {
                    var content = await surveyResponse.Content.ReadAsStringAsync();
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
                    MetricsManager.TrackException("Error reading survey json", ex);
                }

            }
            else
            {
                await PageDialog.AlertAsync("Unable to retrieve survey data", "Error", "OK");
            }
        }

        async Task GetFarms()
        {
            var userId = UserSettings.UserId;
            var farmresponse = await ApiManager.GetFarmsByUserId(userId);

            if (farmresponse.IsSuccessStatusCode || farmresponse.StatusCode == HttpStatusCode.NotModified)
            {
                var response = await farmresponse.Content.ReadAsStringAsync();
                var json = await Task.Run(() => JsonConvert.DeserializeObject<List<FarmDto>>(response));
                FarmList = new ObservableCollection<FarmDto>(json);
            }
            else
            {
                await PageDialog.AlertAsync("Unable to retrieve farm data", "Error", "OK");
            }
        }

        async void NavigateAsync(string page)
        {
            MetricsManager.TrackEvent("Navigate: " + page);
            await NavigationService.NavigateAsync(new Uri(page, UriKind.Absolute));
        }
        async void EditFarm()
        {
            var navigationParams = new NavigationParameters
            {
                { "farm", App.SelectedFarm }
            };
            await NavigationService.NavigateAsync("FarmDetailPage", navigationParams);
        }

        bool CanNavigate(string obj)
        {
            return App.SelectedFarm != null;
        }

        public async override void OnResume()
        {
            base.OnResume();
            ResetButtons();
            //New user with no farms so direct to add farm page
            if (FarmList.Count > 1)
            {
                var result = await _dialogService.DisplayAlertAsync("No farms found", "Would you like to add a farm?", "Yes", "No");
                if (result)
                {
                    await NavigationService.NavigateAsync("ManageFarmsPage");
                }
            }
            else
            {
                if (App.SelectedFarm != null) SelectedFarm = App.SelectedFarm;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            ResetButtons();
        }

        private void ResetButtons()
        {
            FrameMonitorColour = "#ffffff";
            FrameAssessmentColour = "#ffffff";
        }
    }
}
