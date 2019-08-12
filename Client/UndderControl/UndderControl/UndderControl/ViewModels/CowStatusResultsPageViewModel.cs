using Prism.Commands;
using Prism.Mvvm;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace UndderControl.ViewModels
{
    public class CowStatusResultsPageViewModel : BindableBase
    {
        ObservableCollection<ResultData> _results;
        public ObservableCollection<ResultData> NewInfectionRate { get; set; }
        public ObservableCollection<ResultData> InfectionRate { get; set; }
        public ObservableCollection<ResultData> Results
        {
            get { return _results; }
            set
            {
                SetProperty(ref _results, value);
                RaisePropertyChanged("Results");
            }
        }
        string _code;
        public string Code
        {
            get { return _code; }
            private set
            {
                SetProperty(ref _code, value);
                RaisePropertyChanged(nameof(Code));
            }
        }

        public CowStatusResultsPageViewModel()
        {
            ObservableCollection<ResultData> temp = new ObservableCollection<ResultData>
            {
                new ResultData() { Name = "NOT INFECTED AT DRYOFF", Value = 3 },
                new ResultData() { Name = "INFECTED AT DRYOFF", Value = 4 },
                new ResultData() { Name = "NOT INFECTED AFTER CALVING", Value = 5 },
                new ResultData() { Name = "NEW INFECTION", Value = 2 },
                new ResultData() { Name = "FAILURE TO CURE", Value = 1 },
                new ResultData() { Name = "CURE", Value = 3 },
            };
            Results = temp;

            NewInfectionRate = new ObservableCollection<ResultData>
            {
                new ResultData() { Name = "Current Rate", Value = 24 }
            };
            InfectionRate = new ObservableCollection<ResultData>
            {
                new ResultData() { Name = "Current Rate", Value = 76 }
            };

            Code = "<html><body><h1>Xamarin.Forms</h1><p>Welcome to WebView.</p></body></html>";
        }

        public class ResultData
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }
    }
}
