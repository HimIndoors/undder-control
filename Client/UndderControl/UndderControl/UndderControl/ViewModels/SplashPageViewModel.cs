using Prism.Navigation;
using System.Threading.Tasks;
using UndderControl.Helpers;
using UndderControl.Services;
using Xamarin.Essentials;

namespace UndderControl.ViewModels
{
    public class SplashPageViewModel : ViewModelBase
    {
        public SplashPageViewModel(INavigationService navigationService, IMetricsManagerService metricsSevice)
            : base(navigationService, metricsSevice)
        {
            Title = "UnDDER CONTROL";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            InitAsync();
        }

        async void InitAsync()
        {
            await Task.Delay(3000); //Show branding screen for three seconds

            if (UserSettings.UserId <= 0)
                await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
            else
                if (VersionTracking.IsFirstLaunchEver || VersionTracking.IsFirstLaunchForCurrentVersion || Config.TestMode)
                await NavigationService.NavigateAsync("TermsPage");
            else
                await NavigationService.NavigateAsync("/SdctMasterDetailPage/NavigationPage/RootPage");
        }

        
    }
}
