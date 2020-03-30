using Prism.Commands;
using Prism.Navigation;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControl.Text;
using Xamarin.Essentials;

namespace UndderControl.ViewModels
{
    public class TermsPageViewModel : ViewModelBase
    {
        public DelegateCommand OnAcceptCommand { get; private set; }
        public DelegateCommand OnBrowseCommand { get; private set; }
        public TermsPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        { 
            OnAcceptCommand = new DelegateCommand(AcceptTerms);
            OnBrowseCommand = new DelegateCommand(OpenBrowser);
        }

        private async void OpenBrowser()
        {
            await Browser.OpenAsync(AppTextResource.TermsUrl, BrowserLaunchMode.SystemPreferred);
        }

        private async void AcceptTerms()
        {
            UserSettings.TermsVersion = VersionTracking.CurrentVersion; //Capturing the app version currently
            await NavigationService.NavigateAsync("/SdctMasterDetailPage/NavigationPage/RootPage");
        }
    }
}
