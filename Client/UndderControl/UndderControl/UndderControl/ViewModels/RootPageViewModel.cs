using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Services;
using UndderControlLib.Dtos;
using Xamarin.Forms;

namespace UndderControl.ViewModels
{
    public class RootPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IMetricsManagerService _metricsService;
        private ObservableCollection<FarmDto> _farmList;
        private DelegateCommand<string> _navigateCommand;
        private static ISettings AppSettings => CrossSettings.Current;

        public ObservableCollection<FarmDto> FarmList
        {
            get { return _farmList; }
            set
            {
                _farmList = value;
                RaisePropertyChanged("FarmList");
            }
        }
        private FarmDto _selectedFarm { get; set; }
        public FarmDto SelectedFarm
        {
            get { return _selectedFarm; }
            set
            {
                _selectedFarm  = value;
                //Update global farm
                App.SelectedFarm = value;
                OnNavigateCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand<string> OnNavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(NavigateAsync, CanNavigate));

        public RootPageViewModel(INavigationService navigationService, IMetricsManagerService metricsSevice) 
            : base(navigationService)
        {
            _navigationService = navigationService;
            _metricsService = metricsSevice;
            if (App.SelectedFarm != null) SelectedFarm = App.SelectedFarm;
            InitAsync();
        }

        async void InitAsync()
        {
            try
            {
                await RunSafe(GetFarms());
            }
            catch (Exception ex)
            {
                _metricsService.TrackException("GetFarmsFailed", ex);
            }
        }

        async Task GetFarms()
        {
            var userId = 1;
            var farmresponse = await ApiManager.GetFarmsByUserId(userId);

            if (farmresponse.IsSuccessStatusCode)
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
            _metricsService.TrackEvent("Navigate: " + page);
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }

        bool CanNavigate(string obj)
        {
            return App.SelectedFarm == null ? false : true;
        }
    }
}
