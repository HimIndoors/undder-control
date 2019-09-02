using Microcharts;
using Prism.Events;
using SkiaSharp;
using System.Collections.Generic;
using UndderControl.Events;
using UndderControl.Text;
using UndderControl.ViewModels;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace UndderControl.Views
{
    public partial class SurveyResultsPage : ContentPage
    {
        private readonly SurveyResultsPageViewModel _vm;
        public SurveyResultsPage(IEventAggregator ea)
        {
            InitializeComponent();

            FarmSuitability.Text = AppResource.SurveyResultSdctUnsuitable;
            ImprovementTitle.Text = AppResource.SurveyResultImprovementTitle;
            CompareButton.Text = AppResource.SurveyResultCompareButton;

            _vm = BindingContext as SurveyResultsPageViewModel;
            ea.GetEvent<SurveyResultsEvent>().Subscribe(UpdateView);            
        }

        private void UpdateView()
        {
            ResultChart.Chart = new RadarChart() { Entries = _vm.Results };

            if (_vm.Statements != null)
            {
                foreach (var item in _vm.Statements)
                {
                    StatementStack.Children.Add(new Label() { Text = item.Key, FontAttributes = FontAttributes.Bold });
                    foreach (string statement in item.Value)
                    {
                        StatementStack.Children.Add(new Label() { Text = statement });
                    }
                }
            }
        }

        private string ReturnHexValue(int score)
        {
            string hexValue;
            switch (score)
            {
                case 1:
                    hexValue = "#F9AC95";
                    break;
                case 2:
                    hexValue = "#F9AC95";
                    break;
                case 3:
                    hexValue = "#FFDBAA";
                    break;
                case 4:
                    hexValue = "#D3E7AF";
                    break;
                case 5:
                    hexValue = "#D3E7AF";
                    break;
                default:
                    hexValue = "#030303";
                    break;
            }
            return hexValue;
        }
    }
}
