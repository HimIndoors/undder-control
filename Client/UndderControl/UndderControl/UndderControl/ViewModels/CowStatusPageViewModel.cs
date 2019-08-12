using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using UndderControl.Text;

namespace UndderControl.ViewModels
{
    public class CowStatusPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        public DelegateCommand<string> OnDryOffCommand { get; private set; }
        public DelegateCommand<string> OnCalvingCommand { get; private set; }
        public CowStatusPageViewModel(INavigationService navigationService)
            :base (navigationService)
        {
            Title = AppResource.CowStatusPageTitle;
            _navigationService = navigationService;
            OnDryOffCommand = new DelegateCommand<string>(DryOffCommand);
            OnCalvingCommand = new DelegateCommand<string>(CalvingCommand);
        }

        private async void CalvingCommand(string page)
        {
            var navigationParams = new NavigationParameters
            {
                { "mode","calving" }
            };
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative), navigationParams);
        }

        private async void DryOffCommand(string page)
        {
            var navigationParams = new NavigationParameters
            {
                { "mode","dryoff" }
            };
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative), navigationParams);
        }
    }
}
