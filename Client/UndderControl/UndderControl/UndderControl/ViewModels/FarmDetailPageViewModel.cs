using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Services;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class FarmDetailPageViewModel : ViewModelBase
    {
        private FarmDto _currentFarm;
        public FarmDto CurrentFarm
        {
            get { return _currentFarm; }
            private set
            {
                SetProperty(ref _currentFarm, value);
            }
        }
        public DelegateCommand SaveFarmCommand { get; private set; }
        private IMetricsManagerService _metricsManagerService;

        public FarmDetailPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManagerService)
            :base(navigationService)
        {
            _metricsManagerService = metricsManagerService;
            SaveFarmCommand = new DelegateCommand(DoSaveFarm);
        }

        private async void DoSaveFarm()
        {
            try
            {
                await RunSafe(SaveFarm());
            }
            catch (Exception ex)
            {
                await PageDialog.AlertAsync("Unable to save farm data", "Error", "OK");
                _metricsManagerService.TrackException("Error saving farm details", ex);
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
            var farmresponse = await ApiManager.UploadFarm(_currentFarm, false);

            if (!farmresponse.IsSuccessStatusCode)
            { 
                await PageDialog.AlertAsync("Unable to save farm data", "Error", "OK");
            }
        }

    }
}
