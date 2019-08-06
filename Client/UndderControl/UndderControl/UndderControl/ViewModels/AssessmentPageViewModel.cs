using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using UndderControl.Services;

namespace UndderControl.ViewModels
{
    public class AssessmentPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IMetricsManagerService _metricsManagerService;
        public AssessmentPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManagerService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _metricsManagerService = metricsManagerService;
            Title = "Undder Control";
        }
    }
}
