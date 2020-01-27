using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UndderControl.Events;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControlLib.Dtos;
using Xamarin.Forms;

namespace UndderControl.ViewModels
{
    public class ManageFarmsPageViewModel : ViewModelBase
    {
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
                    if (_selectedItem != null)
                        GoToDetail();
                }
            }
        }

        private DelegateCommand<string> _onAddFarmCommand;
        public DelegateCommand<string> AddFarmCommand
            => _onAddFarmCommand ?? (_onAddFarmCommand = new DelegateCommand<string>(AddFarm));
        IEventAggregator _EventAggregator;

        public ManageFarmsPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager, IEventAggregator eventAggregator)
            : base(navigationService, metricsManager)
        {
            _EventAggregator = eventAggregator;
            Title = "FARMS";
            //InitAsync();
        }

        private async void AddFarm(string page)
        {
            MetricsManager.TrackEvent("Navigation: Add Farm");
            await NavigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }

        private async void InitAsync()
        {
            try
            {
                await RunSafe(GetFarms());
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException("GetFarmsFailed", ex);
            }
        }

        async Task GetFarms()
        {
            var serviceResponse = await ApiManager.GetFarmsByUserId(UserSettings.UserId);

            if (serviceResponse.IsSuccessStatusCode || serviceResponse.StatusCode == HttpStatusCode.NotModified)
            {
                var response = await serviceResponse.Content.ReadAsStringAsync();
                var farms = await Task.Run(() => JsonConvert.DeserializeObject<List<FarmDto>>(response));
                Farms = new ObservableCollection<FarmDto>(farms);
            }
        }

        async void GoToDetail()
        {
            var path = "FarmDetailPage";
            MetricsManager.TrackEvent("Navigation: Edit Farm");
            var navigationParams = new NavigationParameters
            {
                { "farm", _selectedItem }
            };
            await NavigationService.NavigateAsync(new Uri(path, UriKind.Relative), navigationParams);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            _EventAggregator.GetEvent<FarmNavigationEvent>().Publish();
            InitAsync();
        }
    }
}
