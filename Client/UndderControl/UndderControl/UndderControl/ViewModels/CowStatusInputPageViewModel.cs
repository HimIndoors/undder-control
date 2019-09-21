using FluentValidation.Results;
using MonkeyCache.SQLite;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UndderControl.Events;
using UndderControl.Services;
using UndderControl.Text;
using UndderControl.Validation;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class CowStatusInputPageViewModel : ViewModelBase, IInitialize
    {
        private DelegateCommand _onNextCommand;
        private DelegateCommand _onFinishCommand;
        public DelegateCommand OnNextCommand => _onNextCommand ?? (_onNextCommand = new DelegateCommand(NextInput));
        public DelegateCommand OnFinishCommand => _onFinishCommand ?? (_onFinishCommand = new DelegateCommand(FinishInput));

        private string _inputMode;
        public string InputMode
        {
            get { return _inputMode; }
            set
            {
                SetProperty(ref _inputMode, value);
                if (_inputMode.Equals("dryoff"))
                {
                    InputModeText = AppTextResource.CowStatusInputTextDryoffStatus;
                }
                else
                {
                    InputModeText = AppTextResource.CowStatusInputTextCalvingStatus;
                }
            }
        }
        private string _inputModeText;
        public string InputModeText
        {
            get { return _inputModeText; }
            set { SetProperty(ref _inputModeText, value); }
        }

        private string _validationErrorMessage;
        public string ValidationErrorMessage
        {
            get { return _validationErrorMessage; }
            set
            {
                SetProperty(ref _validationErrorMessage, value);
                RaisePropertyChanged("ValidationErrorMessage");
            }
        }
        private bool _showValidationErrors = false;
        public bool ShowValidationErrors
        {
            get { return _showValidationErrors; }
            set
            {
                SetProperty(ref _showValidationErrors, value);
                RaisePropertyChanged("ShowValidationErrors");
            }
        }

        public string TestCowId { get; set; }
        public bool CowInfected { get; set; }

        readonly IEventAggregator _EventAggregator;

        public CowStatusInputPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager, IEventAggregator eventAggregator)
            : base(navigationService, metricsManager)
        {
            Title = AppTextResource.CowStatusPageTitle;
            IsBusy = true;
            CowInfected = false; //Default value
            _EventAggregator = eventAggregator;
        }

        private async void NextInput()
        {
            CowStatusDto cs = InitCowStatus();
            CowStatusValidator validator = new CowStatusValidator();
            ValidationResult result = validator.Validate(cs);
            if (result.IsValid)
            {
                await RunSafe(UploadCowStatus(cs));
                _EventAggregator.GetEvent<CowStatusRefreshEvent>().Publish();
            }
            else
            {
                ValidationErrorMessage = result.Errors[0].ErrorMessage;
                ShowValidationErrors = true;
            }

        }

        private async void FinishInput()
        {
            CowStatusDto cs = InitCowStatus();
            CowStatusValidator validator = new CowStatusValidator();
            ValidationResult result = validator.Validate(cs);
            if (result.IsValid)
            {
                await RunSafe(UploadCowStatus(cs));
                if (InputMode.Equals("dryoff"))
                    await NavigationService.NavigateAsync("CowStatusFinishPage");
                else
                {
                    PopulateCowStatusData();
                    await NavigationService.NavigateAsync("CowStatusResultsPage");
                }

            }
            else
            {
                ValidationErrorMessage = result.Errors[0].ErrorMessage;
                ShowValidationErrors = true;
            }
        }

        private async Task UploadCowStatus(CowStatusDto status)
        {
            HttpResponseMessage response;
            if (InputMode.Equals("dryoff"))
            {
                response = await ApiManager.CreateCowStatus(status);
            }
            else
            {
                response = await ApiManager.UpdateCowStatus(status);
            }


            if (!response.IsSuccessStatusCode)
            {
                await PageDialog.AlertAsync("Unable to save cow status data", "Error", "OK");
            }
            else
            {
                PageDialog.Toast("Cow status saved");
            }
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("mode"))
            {
                InputMode = parameters["mode"] as string;
            }
            IsBusy = false;
        }

        private CowStatusDto InitCowStatus()
        {
            CowStatusDto cowStatus = new CowStatusDto();
            if (InputMode.Equals("dryoff"))
            {
                cowStatus.CowIdentifier = TestCowId;
                cowStatus.InfectedAtDryOff = CowInfected;
                cowStatus.Farm_ID = App.SelectedFarm.ID;
                cowStatus.DateAddedDryOff = DateTime.Now;
            }
            else
            {
                cowStatus.CowIdentifier = TestCowId;
                cowStatus.InfectedAtCalving = CowInfected;
                cowStatus.Farm_ID = App.SelectedFarm.ID;
                cowStatus.DateAddedCalving = DateTime.Now;
            }
            return cowStatus;
        }

        private async void PopulateCowStatusData()
        {
            Barrel.Current.Empty(key: "GetCowsStatusByFarmID" + App.SelectedFarm.ID);
            try
            {
                await RunSafe(GetCowStatusData());
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException(ex.Message, ex);
            }
        }

        async Task GetCowStatusData()
        {
            var tryHistoric = false;
            var serviceResponse = await ApiManager.GetCowsStatusByFarmID(App.SelectedFarm.ID);

            if (serviceResponse.IsSuccessStatusCode || serviceResponse.StatusCode == HttpStatusCode.NotModified)
            {
                var response = await serviceResponse.Content.ReadAsStringAsync();
                var cowStatusData = await Task.Run(() => JsonConvert.DeserializeObject<List<CowStatusDto>>(response));
                if (cowStatusData != null && cowStatusData.Count > 0)
                {
                    App.LatestCowStatusData = new List<CowStatusDto>(cowStatusData);
                    tryHistoric = true;
                }
            }

            if (tryHistoric)
            {
                var previousYear = App.LatestCowStatusData[0].DateAddedCalving.Value.Year - 1;
                var historicResponse = await ApiManager.GetCowsStatusByFarmIDandYear(App.SelectedFarm.ID, previousYear);

                if (historicResponse.IsSuccessStatusCode || historicResponse.StatusCode == HttpStatusCode.NotModified)
                {
                    var response = await historicResponse.Content.ReadAsStringAsync();
                    var cowStatusData = await Task.Run(() => JsonConvert.DeserializeObject<List<CowStatusDto>>(response));
                    if (cowStatusData != null && cowStatusData.Count > 0)
                    {
                        App.PreviousCowStatusData = new List<CowStatusDto>(cowStatusData);
                    }
                }
            }
        }
    }
}
