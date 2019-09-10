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
        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> OnNavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(NavigateAsync));
        public SdctMasterDetailPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            Title = "";
        }

        async void NavigateAsync(string page)
        {
            await NavigationService.NavigateAsync(page);
        }
    }
}
