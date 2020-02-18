using Prism.Events;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UndderControl.Custom;
using UndderControl.Events;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginPageViewModel _vm;
        public LoginPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _vm = BindingContext as LoginPageViewModel;
            var loginView = new WebView
            {
                Source = Config.LoginUrl,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            loginView.Navigated += LoginView_Navigated;
            LoginStack.Children.Add(loginView);

            eventAggregator.GetEvent<LoginBackEvent>().Subscribe(UpdateView);
        }

        private void UpdateView()
        {
            WebView loginView = (WebView)LoginStack.Children[0];
            loginView.Source = Config.LoginUrl;
        }

        private async void LoginView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Result == WebNavigationResult.Success)
            {
                var view = sender as WebView;
                var output = await view.EvaluateJavaScriptAsync("document.documentElement.innerHTML");
                _vm.Html = DecodeEncodedNonAsciiCharacters(output);
            }
            
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
    }
}
