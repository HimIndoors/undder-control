using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace UndderControl.Views
{
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage()
        {
            InitializeComponent();
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
