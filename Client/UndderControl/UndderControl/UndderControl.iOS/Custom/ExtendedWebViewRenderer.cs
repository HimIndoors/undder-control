using UIKit;
using UndderControl.Custom;
using UndderControl.iOS.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedWebView), typeof(ExtendedWebViewRenderer))]
namespace UndderControl.iOS.Custom
{
    public class ExtendedWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            Delegate = new ExtendedUIWebViewDelegate(this);
        }
    }

    public class ExtendedUIWebViewDelegate : UIWebViewDelegate
    {
        readonly ExtendedWebViewRenderer webViewRenderer;

        public ExtendedUIWebViewDelegate(ExtendedWebViewRenderer renderer = null)
        {
            webViewRenderer = renderer ?? new ExtendedWebViewRenderer();
        }

        public override async void LoadingFinished(UIWebView webView)
        {
            var wv = webViewRenderer.Element as ExtendedWebView;
            if (wv != null)
            {
                await System.Threading.Tasks.Task.Delay(100).ConfigureAwait(false); // wait here till content is rendered
                if (webView != null)
                    wv.HeightRequest = webView.ScrollView.ContentSize.Height;
            }
        }
    }
}