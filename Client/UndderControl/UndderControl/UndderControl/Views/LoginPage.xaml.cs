using Prism.Events;
using Prism.Navigation;
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
    public partial class LoginPage : ContentPage, INavigationAware
    {
        private readonly LoginPageViewModel _vm;
        private readonly IClearCookies ClearCookies;
        private JsWebView LoginWebView;

        public LoginPage(IEventAggregator eventAggregator, IClearCookies clearCookies)
        {
            InitializeComponent();
            _vm = BindingContext as LoginPageViewModel;
            ClearCookies = clearCookies;

            eventAggregator.GetEvent<LogOutEvent>().Subscribe(LogoutUser);

            //Build(false);
        }

        private void Build(bool disposeView)
        {
            if (disposeView)
                LoginWebView = null; //Avoiding memory leaks

            LoginWebView = new JsWebView
            {
                Source = new UrlWebViewSource
                {
                    Url = Config.LoginUrl
                },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            LoginWebView.Navigated += LoginView_Navigated;
            LoginWebView.Navigating += LoginWebView_Navigating;

            Content = LoginWebView;
        }

        public async void LogoutUser()
        {
            //Fire the logout URL
            LoginWebView = null;
            LoginWebView = new JsWebView
            {
                Source = new UrlWebViewSource
                {
                    Url = Config.LogoutUrl
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            LoginWebView.Navigated += LoginView_Navigated;

            Content = LoginWebView;

            ClearCookies.Clear();

            Build(true);
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

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            var cookies = LoginWebView.Cookies;
            Uri uri = new Uri("https://secure.merck-animal-health.com/");
            foreach (var x in cookies.GetCookies(uri))
            {
                Console.Write("Cookie: " + x.ToString());
            }
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (LoginWebView != null)
            {
                var cookies = LoginWebView.Cookies;
                Uri uri = new Uri("https://secure.merck-animal-health.com/");
                foreach (var x in cookies.GetCookies(uri))
                {
                    Console.Write("Cookie: " + x.ToString());
                }
            }
            

            string pageMode = parameters["mode"] as string;

            if (pageMode != null && pageMode.ToUpper().Equals("LOGOUT"))
            {
                LogoutUser();
            }
            else
            {
                Build(true);
            }
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            var cookies = LoginWebView.Cookies;
            Uri uri = new Uri("https://secure.merck-animal-health.com/");
            foreach (var x in cookies.GetCookies(uri))
            {
                Console.Write("Cookie: " + x.ToString());
            }

            string pageMode = parameters["mode"] as string;

            if (pageMode != null && pageMode.ToUpper().Equals("LOGOUT"))
            {
                LogoutUser();
            }
            else
            {
                Build(true);
            }
        }
    }
}
