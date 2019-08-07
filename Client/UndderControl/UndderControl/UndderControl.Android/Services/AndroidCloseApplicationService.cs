using Android.OS;
using UndderControl.Services;

[assembly: Xamarin.Forms.Dependency(typeof(UndderControl.Droid.Services.AndroidCloseApplicationService))]

namespace UndderControl.Droid.Services
{
    public class AndroidCloseApplicationService : ICloseApplicationService
    {
        public void CloseApplication()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}