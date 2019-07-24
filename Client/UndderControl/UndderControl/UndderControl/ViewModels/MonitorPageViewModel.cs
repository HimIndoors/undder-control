using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UndderControl.ViewModels
{
    public class MonitorPageViewModel : ViewModelBase
    {
        public MonitorPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Undder Control";
        }
    }
}
