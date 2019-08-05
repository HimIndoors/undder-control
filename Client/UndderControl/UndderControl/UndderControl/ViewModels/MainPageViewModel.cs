using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UndderControl.Services;

namespace UndderControl.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IMetricsManagerService _metricsManagerService;
        public MainPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManagerService)
            : base(navigationService, metricsManagerService)
        {
            _navigationService = navigationService;
            _metricsManagerService = metricsManagerService;
            Title = "Undder Control";
        }
    }
}
