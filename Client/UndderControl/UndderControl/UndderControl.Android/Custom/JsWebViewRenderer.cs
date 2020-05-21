using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using UndderControl.Custom;
using UndderControl.Droid.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(JsWebView), typeof(JsWebViewRenderer))]
namespace UndderControl.Droid.Custom
{
    public class JsWebViewRenderer : WebViewRenderer
    {
        public JsWebViewRenderer(Context context) 
            : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var cookieManager = CookieManager.Instance;
                cookieManager.SetAcceptCookie(true);
                cookieManager.SetAcceptThirdPartyCookies(Control, true);

                Control.Settings.JavaScriptEnabled = true;
                Control.Settings.DomStorageEnabled = true;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.Settings.MixedContentMode = MixedContentHandling.AlwaysAllow;

                Control.LoadUrl(Control.Url);

            }  
        }
    }
}