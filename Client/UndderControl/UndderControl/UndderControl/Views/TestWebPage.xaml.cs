using System;
using System.Net.Http;
using UndderControl.Custom;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class TestWebPage : ContentPage
    {
        private Label header;
        private WebView webView;
        private Button button;
        private string queryString = string.Empty;
        private readonly IClearCookies clearCookies;

        public TestWebPage(IClearCookies cc)
        {
            InitializeComponent();
            clearCookies = cc;
            header = new Label
            {
                Text = "WebView",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = Config.LoginUrl
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            webView.Navigated += WebView_Navigated;

            button = new Button
            {
                Text = "Logoff",
                HorizontalOptions = LayoutOptions.Center
            };
            button.Clicked += Button_Clicked;

            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

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

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Result == WebNavigationResult.Success)
            {
                if (e.Url.Contains("?"))
                    queryString = e.Url.Split('?')[1];
            }
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            /*
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = httpClient.GetAsync(Config.LogoutUrl + "?" + queryString).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                //Catch non 200 status codes
            }
            */
            webView = null;
            webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = Config.LogoutUrl
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            webView.Navigated += WebView_Navigated;

            Content = new StackLayout
            {
                Children =
                {
                    header,
                    webView,
                    button
                }
            };

            clearCookies.Clear();

            webView = null;
            webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = Config.LoginUrl
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            webView.Navigated += WebView_Navigated;

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
