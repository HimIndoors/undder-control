using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using UndderControl.Services;

namespace UndderControl.ViewModels
{
    public class SurveyComparisonPageViewModel : ViewModelBase
    {
        public SurveyComparisonPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base (navigationService, metricsManager)
        {
            Title = "";
        }
    }
}
