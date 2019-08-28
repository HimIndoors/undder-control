using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Services;
using Xamarin.Essentials;

namespace UndderControl.ViewModels
{
    public class TermsPageViewModel : ViewModelBase
    {
        IPageDialogService _dialogService;
        INavigationService _navigationService;
        ICloseApplicationService _closeApplicationService;
        IMetricsManagerService _metricsService;
        private static ISettings AppSettings => CrossSettings.Current;
        public DelegateCommand OnAcceptCommand { get; private set; }
        public DelegateCommand OnDeclineCommand { get; private set; }
        public TermsPageViewModel(INavigationService navigationService, 
            IPageDialogService dialogService, 
            IMetricsManagerService metricsService, 
            ICloseApplicationService closeAppService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _closeApplicationService = closeAppService;
            _metricsService = metricsService;

            OnAcceptCommand = new DelegateCommand(AcceptTerms);
            OnDeclineCommand = new DelegateCommand(ConfirmCancel);
        }

        private async void ConfirmCancel()
        {
            var result = await _dialogService.DisplayAlertAsync("Decline", "Select yes to decline terms, the app will close.", "Yes", "No");
            if (result.Equals("Yes"))
            {
                _metricsService.TrackEvent("User didn't accept terms. App closed");
                _closeApplicationService.CloseApplication();
            }
        }

        private async void AcceptTerms()
        {
            AppSettings.AddOrUpdateValue("Terms_" + VersionTracking.CurrentVersion, "User Accepted");
            await _navigationService.NavigateAsync("/SdctMasterDetailPage/NavigationPage/RootPage");
        }
    }
}
