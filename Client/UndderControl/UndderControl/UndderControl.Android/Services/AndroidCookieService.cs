using Android.Webkit;
using UndderControl.Droid.Services;
using UndderControl.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidCookieService))]
namespace UndderControl.Droid.Services
{
    public class AndroidCookieService : ICookieService
    {
        public void Clear()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }
}