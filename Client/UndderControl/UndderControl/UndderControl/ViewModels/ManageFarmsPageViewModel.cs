using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Services;
using UndderControlLib.Dtos;
using Xamarin.Forms;

namespace UndderControl.ViewModels
{
    public class ManageFarmsPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IMetricsManagerService _metricsManagerService;
        private ObservableCollection<FarmDto> _farms;
        public ObservableCollection<FarmDto> Farms
        {
            get { return _farms; }
            set
            {
                _farms = value;
                RaisePropertyChanged("Farms");
            }

        }

        public ManageFarmsPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManagerService)
            : base(navigationService, metricsManagerService)
        {
            _navigationService = navigationService;
            _metricsManagerService = metricsManagerService;
            IsBusy = true;
            InitAsync();
        }

        private async void InitAsync()
        {
            try
            {
                await RunSafe(GetFarms());
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMetricsManagerService>().TrackException("GetFarmsFailed", ex);
            }

            IsBusy = false;
        }

        async Task GetFarms()
        {
            var serviceResponse = await ApiManager.FarmList();

            if (serviceResponse.IsSuccessStatusCode)
            {
                var response = await serviceResponse.Content.ReadAsStringAsync();
                var farms = await Task.Run(() => JsonConvert.DeserializeObject<List<FarmDto>>(response));
                Farms = new ObservableCollection<FarmDto>(farms);
            }
        }
    }
}
