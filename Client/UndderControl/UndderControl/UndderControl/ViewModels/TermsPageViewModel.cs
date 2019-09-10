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
using UndderControl.Helpers;
using UndderControl.Services;
using Xamarin.Essentials;

namespace UndderControl.ViewModels
{
    public class TermsPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly ICloseApplicationService _closeApplicationService;
        public DelegateCommand OnAcceptCommand { get; private set; }
        public DelegateCommand OnDeclineCommand { get; private set; }
        public TermsPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IMetricsManagerService metricsManager, 
            ICloseApplicationService closeAppService)
            : base(navigationService, metricsManager)
        {
            _dialogService = dialogService;
            _closeApplicationService = closeAppService;

            OnAcceptCommand = new DelegateCommand(AcceptTerms);
            OnDeclineCommand = new DelegateCommand(ConfirmCancel);
        }

        private async void ConfirmCancel()
        {
            var result = await _dialogService.DisplayAlertAsync("Decline", "Select yes to decline terms, the app will close.", "Yes", "No");
            if (result.Equals("Yes"))
            {
                MetricsManager.TrackEvent("User didn't accept terms. App closed");
                _closeApplicationService.CloseApplication();
            }
        }

        private async void AcceptTerms()
        {
            UserSettings.TermsVersion = VersionTracking.CurrentVersion; //Capturing the app version currently
            await NavigationService.NavigateAsync("/SdctMasterDetailPage/NavigationPage/RootPage");
        }
    }
}
