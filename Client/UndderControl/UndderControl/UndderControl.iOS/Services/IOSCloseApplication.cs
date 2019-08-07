using System.Threading;
using UndderControl.Services;

[assembly: Xamarin.Forms.Dependency(typeof(UndderControl.iOS.Services.IOSCloseApplicationService))]

namespace UndderControl.iOS.Services
{
    public class IOSCloseApplicationService : ICloseApplicationService
    {
        public void CloseApplication()
        {
            Thread.CurrentThread.Abort();
        }
    }
}