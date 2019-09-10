using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UndderControl.Services;

namespace UndderControl.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                SetProperty(ref _code, value);
                RaisePropertyChanged("Code");
            }
        }

        public MainPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            Code = "<html><body><h1>Hello</h1></body></html>";
        }
    }
}
