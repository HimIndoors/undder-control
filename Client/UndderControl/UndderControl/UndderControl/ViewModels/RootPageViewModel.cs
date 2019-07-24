using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class RootPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
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
                var dbFarms = await App.FarmDatabase.GetAllFarms();
                FarmList = dbFarms;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMetricsManagerService>().TrackException("GetFarmsFailed", ex);
            }

            IsBusy = false;
        }

        async void NavigateAsync(string page)
        {
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }
    }
}
