using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class CowStatusResultsPage : ContentPage
    {
        public CowStatusResultsPage()
        {
            InitializeComponent();
            //Temp Html layout for results
            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><head>
<style>
    body { background:#7CCCBD; }
    table { width:100%; border-collapse: collapse; }
    td { border: 1px solid #ffffff; padding: 5px;}
    .value { text-align: center; }
    .status { margin-top: 1em; background: #00827F; }
    .status td, .status th { color: #ffffff; }

</style>
</head>
<body>
<table>
<tr><td>NOT INFECTED AT DRYOFF</td><td class='value'>3</td></tr>
<tr><td>INFECTED AT DRYOFF</td><td class='value'>4</td></tr>
<tr><td>NOT INFECTED AT CALVING</td><td class='value'>3</td></tr>
<tr><td>INFECTED AT CALVING</td><td class='value'>4</td></tr>
<tr><td>NEW INFECTION</td><td class='value'>3</td></tr>
<tr><td>PREVENTION OF NEW INFECTION</td><td class='value'>4</td></tr>
<tr><td>FAILURE TO CURE</td><td class='value'>3</td></tr>
<tr><td>CURE</td><td class='value'>4</td></tr>
<tr><td>NEW INFECTION RATE (%)</td><td class='value'>67</td></tr>
<tr><td>PREVENTION RATE (%)</td><td class='value'>33</td></tr>
<tr><td>FAILURE TO CURE RATE (%)</td><td class='value'>25</td></tr>
<tr><td>URE RATE (%)</td><td class='value'>75</td></tr>
</table>
<table class='status'>
<tr><th>STATUS</th><th>THRESHOLD</th></tr>
<tr><td>NEW INFECTION</td><td class='value'>10%</td></tr>
<tr><td>PREVENTION OF NEW INFECTION</td><td class='value'>90%</td></tr>
<tr><td>FAILURE TO CURE</td><td class='value'>15%</td></tr>
<tr><td>CURE</td><td class='value'>85%</td></tr>
</table>
</body></html>";
            browser.Source = htmlSource;
            browser.HeightRequest = 650;
            browser.HorizontalOptions = LayoutOptions.Fill;

            WebViewLayout.Children.Add(browser);
        }
    }
}
