using FluentValidation.Results;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using UndderControl.Services;
using UndderControl.Validation;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class FarmDetailPageViewModel : ViewModelBase, IInitialize
    {
        private FarmDto _currentFarm = new FarmDto();
        public FarmDto CurrentFarm
        {
            get { return _currentFarm; }
            private set
            {
                SetProperty(ref _currentFarm, value);
            }
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
        private DelegateCommand _saveFarmCommand;
        public DelegateCommand SaveFarmCommand => _saveFarmCommand ?? (_saveFarmCommand = new DelegateCommand(DoSaveFarm));


        public FarmDetailPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            :base(navigationService, metricsManager)
        {
            Title = "FARM DETAIL";
        }

        private async void DoSaveFarm()
        {
            FarmValidator validator = new FarmValidator();
            ValidationResult result = validator.Validate(_currentFarm);
            if (result.IsValid)
            {
                try
                {
                    await RunSafe(SaveFarm());
                    await NavigationService.GoBackAsync();
                }
                catch (Exception ex)
                {
                    await PageDialog.AlertAsync("Unable to save farm data", "Error", "OK");
                    MetricsManager.TrackException("Error saving farm details", ex);
                }
            }
            else
            {
                ValidationErrorMessage = result.Errors[0].ErrorMessage;
                ShowValidationErrors = true;
            }
            
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters !=null && parameters.ContainsKey("farm"))
            {
                CurrentFarm = parameters["farm"] as FarmDto;
            }
        }

        async Task SaveFarm()
        {
            var response = await ApiManager.UploadFarm(_currentFarm, false);

            if (!response.IsSuccessStatusCode)
            { 
                await PageDialog.AlertAsync("Unable to save cow status data", "Error", "OK");
            }
        }
    }
}
