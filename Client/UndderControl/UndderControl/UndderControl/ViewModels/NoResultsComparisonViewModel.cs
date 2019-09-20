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
    public class NoResultsComparisonPageViewModel : ViewModelBase
    {
        public NoResultsComparisonPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            Title = AppTextResource.NoResultsComparePageTitle;
        }
    }
}
