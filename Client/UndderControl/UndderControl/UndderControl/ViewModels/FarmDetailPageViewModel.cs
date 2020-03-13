using FluentValidation.Results;
using MonkeyCache.SQLite;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UndderControl.Helpers;
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
            set
            {
                SetProperty(ref _currentFarm, value);
                RaisePropertyChanged("CurrentFarm");
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

        private ObservableCollection<FarmTypeDto> farmTypes;
        public ObservableCollection<FarmTypeDto> FarmTypes
        {
            get { return farmTypes; }
            set
            {
                SetProperty(ref farmTypes, value);
                RaisePropertyChanged("FarmTypes");
            }
        }
        private FarmTypeDto selectedType;
        public FarmTypeDto SelectedType
        {
            get { return selectedType; }
            set
            {
                SetProperty(ref selectedType, value);
                RaisePropertyChanged("SelectedType");
            }
        }

        public FarmDetailPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            :base(navigationService, metricsManager)
        {
            Title = "FARM DETAIL";
            InitAsync();
        }

        private async void InitAsync()
        {
            try
            {
                await RunSafe(GetFarmTypes());
            }
            catch (Exception ex)
            {
                await PageDialog.AlertAsync("Unable to load farm types", "Error", "OK");
                MetricsManager.TrackException("Error loading farm types", ex);
            }

            if (CurrentFarm.FarmType_ID > 0)
                SelectedType = FarmTypes.DefaultIfEmpty(null).Where(x => x.ID == CurrentFarm.FarmType_ID).FirstOrDefault();
        }

        private async Task GetFarmTypes()
        {
            var serviceResponse = await ApiManager.GetFarmTypes();

            if (serviceResponse.IsSuccessStatusCode || serviceResponse.StatusCode == HttpStatusCode.NotModified)
            {
                try
                {
                    var response = await serviceResponse.Content.ReadAsStringAsync();
                    var farmTypeData = await Task.Run(() => JsonConvert.DeserializeObject<List<FarmTypeDto>>(response));
                    FarmTypes = new ObservableCollection<FarmTypeDto>(farmTypeData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private async void DoSaveFarm()
        {
            //Add Farmtype from picker and user id from UserSettings
            if (SelectedType != null)
            {
                _currentFarm.FarmType_ID = SelectedType.ID;
            }  
            _currentFarm.User_ID = UserSettings.UserId;

            FarmValidator validator = new FarmValidator();
            ValidationResult result = validator.Validate(_currentFarm);
            if (result.IsValid)
            {
                try
                {
                    if (!Config.TestMode) await RunSafe(SaveFarm());
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
                try
                {
                    CurrentFarm = parameters["farm"] as FarmDto;
                }
                catch(Exception ex)
                {
                    MetricsManager.TrackException(ex.Message, ex);
                }
            }
        }

        async Task SaveFarm()
        {
            var isNew = _currentFarm.ID == 0 ? true : false;
            var response = await ApiManager.UploadFarm(_currentFarm, isNew);

            if (!response.IsSuccessStatusCode || !(response.StatusCode == HttpStatusCode.NoContent))
            { 
                await PageDialog.AlertAsync("Unable to save farm details", "Error", "OK");
            }
            else
            {
                //Empty MonkeyCache farmlist for this user.
                Barrel.Current.Empty(key: "GetFarmsByUserId" + UserSettings.UserId);
                PageDialog.Toast("Farm Saved");
            } 
        }
    }
}
