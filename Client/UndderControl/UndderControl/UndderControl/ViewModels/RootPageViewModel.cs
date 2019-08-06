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
    public class RootPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IMetricsManagerService _metricsManagerService;
        ObservableCollection<FarmDto> _farmList;
        public ObservableCollection<FarmDto> FarmList
        {
            get { return _farmList; }
            set
            {
                _farmList = value;
                RaisePropertyChanged("FarmList");
            }
        }
        public DelegateCommand<string> OnNavigateCommand { get; set; }

        public RootPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            
            OnNavigateCommand = new DelegateCommand<string>(NavigateAsync);
            IsBusy = true;
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
                _metricsManagerService.TrackException("GetFarmsFailed", ex);
            }

            IsBusy = false;
        }

        async Task GetFarms()
        {
            var farmresponse = await ApiManager.FarmList();

            if (farmresponse.IsSuccessStatusCode)
            {
                var response = await farmresponse.Content.ReadAsStringAsync();
                var json = await Task.Run(() => JsonConvert.DeserializeObject<List<FarmDto>>(response));
                _farmList = new ObservableCollection<FarmDto>(json);
            }
            else
            {
                await PageDialog.AlertAsync("Unable to retrieve farm data", "Error", "OK");
            }

        }

        async void NavigateAsync(string page)
        {
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }
    }
}
