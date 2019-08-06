using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using UndderControl.Services;

namespace UndderControl.ViewModels
{
    public class SdctMasterDetailPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IMetricsManagerService _metricsManagerService;
        public DelegateCommand<string> OnNavigateCommand { get; set; }
        public SdctMasterDetailPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManagerService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _metricsManagerService = metricsManagerService;
            OnNavigateCommand = new DelegateCommand<string>(NavigateAsync);
        }

        async void NavigateAsync(string page)
        {
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }
    }
}
