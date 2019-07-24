using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UndderControl.ViewModels
{
    public class SdctMasterDetailPageViewModel : BindableBase
    {
        INavigationService _navigationService;
        public DelegateCommand<string> OnNavigateCommand { get; set; }
        public SdctMasterDetailPageViewModel(INavigationService navigationservice)
        {
            _navigationService = navigationservice;
            OnNavigateCommand = new DelegateCommand<string>(NavigateAsync);
        }

        async void NavigateAsync(string page)
        {
            await _navigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }
    }
}
