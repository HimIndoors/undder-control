using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using UndderControl.Services;
using UndderControl.Text;

namespace UndderControl.ViewModels
{
    public class NoResultsComparisonViewModel : ViewModelBase
    {
        public NoResultsComparisonViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            Title = AppResource.NoResultsComparePageTitle;
        }
    }
}
