using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using UndderControl.Text;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace UndderControl.Views
{
    public partial class SurveyResultsPage : ContentPage
    {
        Dictionary<string, List<string>> _statements;
        public SurveyResultsPage()
        {
            InitializeComponent();

            FarmSuitability.Text = AppResource.SurveyResultSdctUnsuitable;
            ImprovementTitle.Text = AppResource.SurveyResultImprovementTitle;


            //TEMP: Data set to show full chart.
            List<Entry> entries = new List<Entry>()
            {
                new Entry(4)
                {
                    Label = "Dry-off Prep",
                    ValueLabel = "4",
                    Color = SKColor.Parse(ReturnHexValue(4)),
                },
                new Entry(5)
                {
                    Label = "Dry-off",
                    ValueLabel = "5",
                    Color = SKColor.Parse(ReturnHexValue(5)),
                },
                new Entry(2)
                {
                    Label = "Far Off",
                    ValueLabel = "2",
                    Color = SKColor.Parse(ReturnHexValue(2)),
                },
                new Entry(1)
                {
                    Label = "Close Up",
                    ValueLabel = "1",
                    Color = SKColor.Parse(ReturnHexValue(1)),
                },
                new Entry(4)
                {
                    Label = "Calfing",
                    ValueLabel = "4",
                    Color = SKColor.Parse(ReturnHexValue(4)),
                }

            };
            ResultChart.Chart = new RadarChart() { Entries = entries };

            _statements = new Dictionary<string, List<string>>();
            _statements.Add(
                "DRYOFF PREPARATION", 
                new List<string> {"Poor teat-end condition should be present in less than 15% of cows at dry off." }
                );
            _statements.Add(
                "DRYOFF",
                new List<string> {
                    "You should be using cow somatic cell counts or another reliable test to diagnose infection.",
                    "Your antibiotic and/or teat seal tube selection should be based on well-supported data.",
                    "You should mitigate potential stressors, such as commingling, ample space per cow, access to feed and access to water."
                });
            _statements.Add(
                "FAR OFF",
                new List<string> {
                    "You must ensure cows’ udders and thighs are clean.",
                    "Tails must be clipped, udders shaven as needed and bedding refreshed and disinfected regularly.",
                    "You need to calculate ration, fed ration, eaten ration and dry matter intake are the same and are meeting standard nutritional requirements."
                });
            _statements.Add(
                "CLOSE UP",
                new List<string> {
                    "You must ensure cows’ udders and thighs are clean.",
                    "Commingling and overcrowding should be minimised."
                });
            _statements.Add(
                "CALVING",
                new List<string> {
                    "Less than 5% of cows should be showing visible milk leakage.",
                    "Less than 10% of calvings should need assistance."
                });

            foreach(var item in _statements)
            {
                StatementStack.Children.Add(new Label() { Text = item.Key, FontAttributes=FontAttributes.Bold });
                foreach (string statement in item.Value)
                {
                    StatementStack.Children.Add(new Label() { Text = statement });
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
