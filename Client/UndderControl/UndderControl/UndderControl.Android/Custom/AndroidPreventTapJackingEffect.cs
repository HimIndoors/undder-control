using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ResolutionGroupName ("PMN")]
[assembly:ExportEffect (typeof(UndderControl.Droid.Custom.AndroidPreventTapJackingEffect), nameof(UndderControl.Droid.Custom.AndroidPreventTapJackingEffect))]
namespace UndderControl.Droid.Custom
{
    public class AndroidPreventTapJackingEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            Control.FilterTouchesWhenObscured = true;
        }

        protected override void OnDetached()
        {
            Control.FilterTouchesWhenObscured = false;
        }
    }
}