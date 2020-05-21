using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace UndderControl.Custom
{
    public delegate void WebViewNavigatedHandler(object sender, CookieNavigatedEventArgs args);
    public delegate void WebViewNavigatingHandler(object sender, CookieNavigationEventArgs args);

    public class CookieWebView : WebView
    {
        public CookieCollection Cookies { get; protected set; }

        public event WebViewNavigatedHandler Navigated;
        public event WebViewNavigatingHandler Navigating;

        public virtual void OnNavigated(CookieNavigatedEventArgs args)
        {
            var eventHandler = Navigated;

            if (eventHandler != null)
            {
                eventHandler(this, args);
                Cookies = args.Cookies;
            }
        }

        public virtual void OnNavigating(CookieNavigationEventArgs args)
        {
            Navigating?.Invoke(this, args);
        }
    }

    public class CookieNavigationEventArgs : EventArgs
    {
        public string Url { get; set; }
    }

    public class CookieNavigatedEventArgs : CookieNavigationEventArgs
    {
        public CookieCollection Cookies { get; set; }
        public CookieNavigationMode NavigationMode { get; set; }
    }

    public enum CookieNavigationMode
    {
        Back,
        Forward,
        New,
        Refresh,
        Reset
    }
}
