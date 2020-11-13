using Android.Content;
using UndderControl.Droid.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ContentPage), typeof(AndroidPreventTapJackingRenderer))]
namespace UndderControl.Droid.Custom
{
    public class AndroidPreventTapJackingRenderer : PageRenderer
    {
        public AndroidPreventTapJackingRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            base.RootView.FilterTouchesWhenObscured = true;
            base.FilterTouchesWhenObscured = true;
        }
    }
}