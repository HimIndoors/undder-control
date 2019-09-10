using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using UndderControl.Services;

namespace UndderControl.ViewModels
{
    public class MonitorPageViewModel : ViewModelBase
    {
        private DelegateCommand<string> _onNavigateCommand;
        public DelegateCommand<string> OnNavigateCommand
            => _onNavigateCommand ?? (_onNavigateCommand = new DelegateCommand<string>(NavigateAsync));
        public DelegateCommand OnStatusCommand { get; private set; }

        public MonitorPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            Title = "";
        }

        private async void NavigateAsync(string page)
        {
            await NavigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }
    }
}
