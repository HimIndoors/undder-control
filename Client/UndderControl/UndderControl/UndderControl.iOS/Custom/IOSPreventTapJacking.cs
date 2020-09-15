using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("PMN")]
[assembly: ExportEffect(typeof(UndderControl.iOS.Custom.IOSPreventTapJackingEffect), nameof(UndderControl.iOS.Custom.IOSPreventTapJackingEffect))]
namespace UndderControl.iOS.Custom
{
    public class IOSPreventTapJackingEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            //Android fix required only
        }

        protected override void OnDetached()
        {
            //Android fix required only
        }
    }
}