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
        private FarmDto _selectedItem;
        public FarmDto SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    GoToDetail();
                }
            }
        }
        public DelegateCommand<string> AddFarmCommand { get; private set; }

        public ManageFarmsPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManagerService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _metricsManagerService = metricsManagerService;
            AddFarmCommand = new DelegateCommand<string>(AddFarm);
            InitAsync();
        }

        private async void AddFarm(string page)
        {
            _metricsManagerService.TrackEvent("Navigation: Add Farm");
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }

        private async void InitAsync()
        {
            try
            {
                await RunSafe(GetFarms());
            }
            catch (Exception ex)
            {
                _metricsManagerService.TrackException("GetFarmsFailed", ex);
            }
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

        async void GoToDetail()
        {
            var path = "NavigationPage/FarmDetailPage";
            _metricsManagerService.TrackEvent("Navigation: Edit Farm");
            var navigationParams = new NavigationParameters
            {
                { "farm", _selectedItem }
            };
            await _navigationService.NavigateAsync(new Uri(path, UriKind.Relative), navigationParams);
        }
    }
}
