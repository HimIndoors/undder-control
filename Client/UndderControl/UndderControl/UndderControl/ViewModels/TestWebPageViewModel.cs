using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using UndderControl.Services;

namespace UndderControl.ViewModels
{
    public class TestWebPageViewModel : ViewModelBase
    {
        private string _html;
        public string Html
        {
            get { return _html; }
            set
            {
                _html = value;
                RaisePropertyChanged();
            }
        }
        public TestWebPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {

        }
    }
}
