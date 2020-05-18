using Prism.Events;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UndderControl.Custom;
using UndderControl.Events;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginPageViewModel _vm;
        private readonly IClearCookies ClearCookies;
        private string QueryString = string.Empty;

        public LoginPage(IEventAggregator eventAggregator, IClearCookies clearCookies)
        {
            InitializeComponent();
            _vm = BindingContext as LoginPageViewModel;
            ClearCookies = clearCookies;
            LoginWebView.Source = Config.LoginUrl;
            eventAggregator.GetEvent<LogOutEvent>().Subscribe(LogoutUser);
            eventAggregator.GetEvent<EndUserSessionEvent>().Subscribe(ResetWebView);
        }

        private void ResetWebView()
        {
            //Send logoff http request
            DoLogout();

            //Dispose the webview
        }

        public async void LogoutUser()
        {
            await Task.Run(() => DoLogout());
            await Task.Run(() => ClearCookies.Clear());
            await Task.Run(() => ResetView());
        }

        private void DoLogout()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync(Config.LogoutUrl + "?" + QueryString).Result;
            response.EnsureSuccessStatusCode();
        }

        private void ResetView()
        {
            LoginWebView.Source = new UrlWebViewSource { Url = Config.LoginUrl };        
        }

        private async void LoginView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            _vm.PageDialog.HideLoading();
            if (e.Result == WebNavigationResult.Success)
            {
                if (e.Url.Contains("?"))
                    QueryString = e.Url.Split('?')[1];
                var view = sender as WebView;
                var output = await view.EvaluateJavaScriptAsync("document.documentElement.innerHTML");
                _vm.Html = DecodeEncodedNonAsciiCharacters(output);
            }  
        }

        private void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            ResetView();
        }

        static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }

        private void LoginWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            _vm.PageDialog.ShowLoading("Loading");
        }
    }
}
