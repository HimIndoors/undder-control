using Prism.Events;
using System;
using System.Text;
using UndderControl.Custom;
using UndderControl.Events;
using UndderControl.Text;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class CowStatusResultsPage : ContentPage
    {
        private readonly CowStatusResultsPageViewModel _vm;
        public CowStatusResultsPage()
        {
            InitializeComponent();
            _vm = BindingContext as CowStatusResultsPageViewModel;
            FarmName.Text = App.SelectedFarm.Name;
            BuildTable();
        }

        private void BuildTable()
        {
            var browser = new ExtendedWebView();
            var htmlSource = new HtmlWebViewSource();
            //Setup percentage values
            var newInfectionRate = (int)Math.Round((double)(100 * _vm.Results[AppResource.CsNewInfection]) / _vm.Results[AppResource.CsNotInfectedAtDryoff]);
            var preventionRate = (int)Math.Round((double)(100 * _vm.Results[AppResource.CsPreventionOfNewInfection]) / _vm.Results[AppResource.CsNotInfectedAtDryoff]);
            var failureToCureRate = (int)Math.Round((double)(100 * _vm.Results[AppResource.CsFailureToCure]) / _vm.Results[AppResource.CsInfectedAtDryoff]);
            var cureRate = (int)Math.Round((double)(100 * _vm.Results[AppResource.CsCure]) / _vm.Results[AppResource.CsInfectedAtDryoff]);

            StringBuilder sb = new StringBuilder(
                @"<html><head>
                <style>
                    body { background:#7CCCBD; }
                    table { width:100%; border-collapse: collapse; border: 1px solid #ffffff; }
                    td { padding: 5px; font-weight: 700; }
                    .value { text-align: center; border-left: 1px solid #ffffff; }
                    .status { margin-top: 1em; background: #00827F; }
                    .status td, .status th { color: #ffffff; }
                    .status th { border-bottom: 1px solid #ffffff; }
                    .section td { border-top: 1px solid #ffffff; }
                </style>
                </head>
                <body>
                <table>");
            sb.AppendLine("<tr><td>" + AppResource.CsNotInfectedAtDryoff + "</td><td class='value'>" + _vm.Results[AppResource.CsNotInfectedAtDryoff] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppResource.CsInfectedAtDryoff + "</td><td class='value'>" + _vm.Results[AppResource.CsInfectedAtDryoff] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppResource.CsNotInfectedAfterCalving + "</td><td class='value'>" + _vm.Results[AppResource.CsNotInfectedAfterCalving] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppResource.CsInfectedAfterCalving + "</td><td class='value'>" + _vm.Results[AppResource.CsInfectedAfterCalving] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppResource.CsNewInfection + "</td><td class='value'>" + _vm.Results[AppResource.CsNewInfection] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppResource.CsPreventionOfNewInfection + "</td><td class='value'>" + _vm.Results[AppResource.CsPreventionOfNewInfection] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppResource.CsFailureToCure + "</td><td class='value'>" + _vm.Results[AppResource.CsFailureToCure] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppResource.CsCure + "</td><td class='value'>" + _vm.Results[AppResource.CsCure] + "</td></tr>");
            sb.AppendLine("<tr class='section'><td>NEW INFECTION RATE (%)</td><td class='value'>" + newInfectionRate + "</td></tr>");
            sb.AppendLine("<tr><td>PREVENTION RATE (%)</td><td class='value'>" + preventionRate + "</td></tr>");
            sb.AppendLine("<tr><td>FAILURE TO CURE RATE (%)</td><td class='value'>" + failureToCureRate + "</td></tr>");
            sb.AppendLine("<tr><td>CURE RATE (%)</td><td class='value'>" + cureRate + "</td></tr>");
            sb.AppendLine("</table><table class='status'><tr><th>STATUS</th><th>THRESHOLD<br/>(PER FARM)</th></tr>");
            
            sb.AppendLine("<tr><td>NEW INFECTION</td><td class='value'><span>10%</span></td></tr>");
            sb.AppendLine("<tr><td>PREVENTION RATE (%)</td><td class='value'><span>90%</span></td></tr>");
            sb.AppendLine("<tr><td>FAILURE TO CURE RATE (%)</td><td class='value'><span>15%</span></td></tr>");
            sb.AppendLine("<tr><td>CURE RATE (%)</td><td class='value'><span>85%</span></td></tr>");
            sb.AppendLine("</table></body></html>");

            htmlSource.Html = sb.ToString();
            browser.Source = htmlSource;
            browser.HeightRequest = 20;
            browser.HorizontalOptions = LayoutOptions.Fill;

            WebViewLayout.Children.Add(browser);
        }
    }

    
}
