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
        INavigationService _navigationService;

        public MonitorPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Undder Control";
        }
    }
}
