using Android.Webkit;
using UndderControl.Custom;
using UndderControl.Droid.Custom;
using Xamarin.Forms;

[assembly: Dependency(typeof(IClearCookiesImplementation))]
namespace UndderControl.Droid.Custom
{
    public class IClearCookiesImplementation : IClearCookies
    {
        public void Clear()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }
}