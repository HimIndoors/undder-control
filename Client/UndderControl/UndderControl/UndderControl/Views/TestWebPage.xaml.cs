using System;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using UndderControl.Custom;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class TestWebPage : ContentPage
    {
        private readonly IClearCookies clearCookies;
        private readonly TestWebPageViewModel _vm;

        private Label header;
        private JsWebView webView;
        private Button button;

        public TestWebPage(IClearCookies cc)
        {
            InitializeComponent();
            clearCookies = cc;
            _vm = BindingContext as TestWebPageViewModel;

            header = new Label
            {
                Text = "WebView",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            button = new Button
            {
                Text = "Logoff",
                HorizontalOptions = LayoutOptions.Center
            };
            button.Clicked += Button_Clicked;

            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            Build(false);
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
            webView.Source = new UrlWebViewSource { Url = Config.LoginUrl };
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            clearCookies.Clear();
            Build(true);
        }

        private void Build(bool disposeView)
        {
            if (disposeView) { }
                webView = null; //Avoiding memory leaks

            webView = new JsWebView
            {
                Source = new UrlWebViewSource
                {
                    Url = Config.LoginUrl
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            webView.Navigated += LoginView_Navigated;
            webView.Navigating += LoginWebView_Navigating;

            Content = new StackLayout
            {
                Children =
                {
                    header,
                    webView,
                    button
                }
            };
        }
    }
}
