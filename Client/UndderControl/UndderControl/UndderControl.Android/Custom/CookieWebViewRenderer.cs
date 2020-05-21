using System.Net;
using Android.Content;
using Android.Graphics;
using Android.Webkit;
using UndderControl.Custom;
using UndderControl.Droid.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Xamarin.Forms.WebView;

[assembly: ExportRenderer(typeof(CookieWebView), typeof(CookieWebViewRenderer))]
namespace UndderControl.Droid.Custom
{
    public class CookieWebViewRenderer : WebViewRenderer
    {
        public CookieWebViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.SetWebViewClient(new CookieWebViewClient(CookieWebView));
            }
        }

        public CookieWebView CookieWebView
        {
            get { return Element as CookieWebView; }
        }
    }

    internal class CookieWebViewClient : WebViewClient
    {
        private readonly CookieWebView _cookieWebView;
        internal CookieWebViewClient(CookieWebView cookieWebView)
        {
            _cookieWebView = cookieWebView;
        }

        public override void OnPageStarted(global::Android.Webkit.WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);

            _cookieWebView.OnNavigating(new CookieNavigationEventArgs
            {
                Url = url
            });
        }

        public override void OnPageFinished(global::Android.Webkit.WebView view, string url)
        {
            var cookieHeader = CookieManager.Instance.GetCookie(url);
            var cookies = new CookieCollection();
            var cookiePairs = cookieHeader.Split('&');
            foreach (var cookiePair in cookiePairs)
            {
                var cookiePieces = cookiePair.Split('=');
                if (cookiePieces[0].Contains(":"))
                    cookiePieces[0] = cookiePieces[0].Substring(0, cookiePieces[0].IndexOf(":"));
                cookies.Add(new Cookie
                {
                    Name = cookiePieces[0],
                    Value = cookiePieces[1]
                });
            }

            _cookieWebView.OnNavigated(new CookieNavigatedEventArgs
            {
                Cookies = cookies,
                Url = url
            });
        }
    }
}