using Prism.Events;
using Prism.Navigation;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UndderControl.Custom;
using UndderControl.Events;
using UndderControl.Services;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class LoginPage : ContentPage, INavigationAware
    {
        private readonly LoginPageViewModel _vm;

        public LoginPage()
        {
            InitializeComponent();
            _vm = BindingContext as LoginPageViewModel;
            LoginWebView.Source = new UrlWebViewSource { Url = Config.LoginUrl };
        }

        private async void LoginView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            _vm.PageDialog.HideLoading();
            if (e.Result == WebNavigationResult.Success)
            {
                var view = sender as WebView;
                var output = await view.EvaluateJavaScriptAsync("document.documentElement.innerHTML");
                _vm.Html = DecodeEncodedNonAsciiCharacters(output);
            }  
        }

        private void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            LoginWebView.Source = new UrlWebViewSource { Url = Config.LoginUrl };
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

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            LoginWebView.Source = new UrlWebViewSource { Url = Config.LoginUrl };
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }
    }
}
