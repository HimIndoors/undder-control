using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using UndderControl.Services;

namespace UndderControl.ViewModels
{
    public class ConnectionIssuePageViewModel : ViewModelBase
    {
        public DelegateCommand RetryCommand { get; }

        public ConnectionIssuePageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            RetryCommand = new DelegateCommand(RetryLogin);
        }

        private async void RetryLogin()
        {
            await NavigationService.NavigateAsync("LoginPage");
        }
    }
}
