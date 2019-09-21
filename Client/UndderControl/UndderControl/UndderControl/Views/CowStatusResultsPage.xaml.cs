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
            var newInfectionRate = (int)Math.Round((double)(100 * _vm.Results[AppTextResource.CsNewInfection]) / _vm.Results[AppTextResource.CsNotInfectedAtDryoff]);
            var preventionRate = (int)Math.Round((double)(100 * _vm.Results[AppTextResource.CsPreventionOfNewInfection]) / _vm.Results[AppTextResource.CsNotInfectedAtDryoff]);
            var failureToCureRate = (int)Math.Round((double)(100 * _vm.Results[AppTextResource.CsFailureToCure]) / _vm.Results[AppTextResource.CsInfectedAtDryoff]);
            var cureRate = (int)Math.Round((double)(100 * _vm.Results[AppTextResource.CsCure]) / _vm.Results[AppTextResource.CsInfectedAtDryoff]);

            StringBuilder sb = new StringBuilder(
                @"<html><head>
                <style>
                    body { background:#7CCCBD; color: #30454A; margin: 0; padding: 0; box-sizing: border-box; }
                    table { width:99.5%; border-collapse: collapse; border: 1px solid #ffffff; margin: 0; box-sizing: border-box; }
                    td, th { padding: 8px; font-weight: 700; }
                    .value { text-align: center; border-left: 1px solid #ffffff; }
                    .value span { background: #ffffff; padding: 3px 6px; color: #30454A !important;}
                    .status { margin-top: 1em; background: #00827F; }
                    .status td, .status th { color: #ffffff; }
                    .status th { border-bottom: 1px solid #ffffff; }
                    .section td { border-top: 1px solid #ffffff; }
                </style>
                </head>
                <body>
                <table>");
            sb.AppendLine("<tr><td>" + AppTextResource.CsNotInfectedAtDryoff + "</td><td class='value'>" + _vm.Results[AppTextResource.CsNotInfectedAtDryoff] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppTextResource.CsInfectedAtDryoff + "</td><td class='value'>" + _vm.Results[AppTextResource.CsInfectedAtDryoff] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppTextResource.CsNotInfectedAfterCalving + "</td><td class='value'>" + _vm.Results[AppTextResource.CsNotInfectedAfterCalving] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppTextResource.CsInfectedAfterCalving + "</td><td class='value'>" + _vm.Results[AppTextResource.CsInfectedAfterCalving] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppTextResource.CsNewInfection + "</td><td class='value'>" + _vm.Results[AppTextResource.CsNewInfection] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppTextResource.CsPreventionOfNewInfection + "</td><td class='value'>" + _vm.Results[AppTextResource.CsPreventionOfNewInfection] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppTextResource.CsFailureToCure + "</td><td class='value'>" + _vm.Results[AppTextResource.CsFailureToCure] + "</td></tr>");
            sb.AppendLine("<tr><td>" + AppTextResource.CsCure + "</td><td class='value'>" + _vm.Results[AppTextResource.CsCure] + "</td></tr>");
            sb.AppendLine("<tr class='section'><td>NEW INFECTION RATE (%)</td><td class='value'>" + newInfectionRate + "</td></tr>");
            sb.AppendLine("<tr><td>PREVENTION RATE (%)</td><td class='value'>" + preventionRate + "</td></tr>");
            sb.AppendLine("<tr><td>FAILURE TO CURE RATE (%)</td><td class='value'>" + failureToCureRate + "</td></tr>");
            sb.AppendLine("<tr><td>CURE RATE (%)</td><td class='value'>" + cureRate + "</td></tr>");
            sb.AppendLine("</table><table class='status'><tr><th>STATUS</th><th class='value'>THRESHOLD<br/>(PER FARM)</th></tr>");
            
            sb.AppendLine("<tr><td>NEW INFECTION</td><td class='value'><span>10%</span></td></tr>");
            sb.AppendLine("<tr><td>PREVENTION RATE (%)</td><td class='value'><span>90%</span></td></tr>");
            sb.AppendLine("<tr><td>FAILURE TO CURE RATE (%)</td><td class='value'><span>15%</span></td></tr>");
            sb.AppendLine("<tr><td>CURE RATE (%)</td><td class='value'><span>85%</span></td></tr>");
            sb.AppendLine("</table></body></html>");

            htmlSource.Html = sb.ToString();
            browser.Source = htmlSource;
            browser.HeightRequest = 200;
            browser.HorizontalOptions = LayoutOptions.Fill;

            WebViewLayout.Children.Add(browser);
        }
    }

    
}
