using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginPageViewModel _vm;
        public LoginPage()
        {
            InitializeComponent();
            _vm = BindingContext as LoginPageViewModel;
            var loginView = new WebView
            {
                Source = "http://lfwmobilehybrid.merck-animal-health.com/LoginCheck.asp?lfwmobileapp=lfwmodetest",
                HeightRequest = 400,
                WidthRequest = 400
            };
            loginView.Navigated += LoginView_Navigated;
            LoginStack.Children.Add(loginView);
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
