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
        //ICloseApplicationService _closeApplicationService;
        private static ISettings AppSettings => CrossSettings.Current;
        private string _termData;
        public string TermData
        {
            get { return _termData; }
            set { SetProperty(ref _termData, value); }
        }
        public DelegateCommand OnAcceptCommand { get; private set; }
        public DelegateCommand OnDeclineCommand { get; private set; }
        public TermsPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            //_closeApplicationService = closeApplicationService;
            OnAcceptCommand = new DelegateCommand(AcceptTerms);
            OnDeclineCommand = new DelegateCommand(ConfirmCancel);
            //Temp load garbage into terms, not sure if this should be an embedded html resource or retrieved from the DB.
            TermData = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed bibendum porttitor neque ac cursus. Donec pellentesque non lorem ullamcorper varius. Morbi in lacinia purus. Praesent orci metus, porta eleifend turpis eget, lacinia mollis risus. Aliquam in ullamcorper purus. Proin dignissim cursus massa, eu placerat nibh gravida id. Maecenas in auctor elit. In vitae lacus at est aliquam efficitur a non enim. Donec sit amet erat fringilla, ultrices augue sed, imperdiet sem. Proin ac massa eget lectus tempus facilisis. Nulla facilisi. Maecenas sit amet purus ornare, pulvinar enim ut, varius felis. Nulla eget lacinia est, ac lacinia nunc. Integer ut enim in tellus auctor luctus. Vestibulum porttitor et lorem non dapibus. Nam posuere egestas turpis, elementum dignissim nunc mattis a. Integer nec tristique leo. Suspendisse scelerisque dolor nec congue iaculis. Integer tristique ipsum dolor, ac sodales eros viverra eget. Phasellus at consectetur quam, eu pellentesque lacus. Proin bibendum risus a arcu aliquam, id vestibulum leo fermentum.Integer mi eros, maximus sit amet erat sed, fermentum faucibus dui.Nullam rhoncus, nibh ut porta dapibus, mauris nunc congue metus, in viverra turpis eros eget mauris.Curabitur lobortis bibendum nunc. Sed sed dictum diam. In non ante eget metus aliquet aliquet.Suspendisse malesuada turpis neque, ac pharetra nibh eleifend in. Phasellus condimentum magna sit amet ornare vestibulum.";

        }

        private async void ConfirmCancel()
        {
            var result = await _dialogService.DisplayAlertAsync("Decline", "Select yes to decline terms, the app will close.", "Yes", "No");
            if (result.Equals("Yes"))
            {
                //_closeApplicationService.CloseApplication();
                await _dialogService.DisplayAlertAsync("Dead", "App closed here", "OK", null);
            }
        }

        private async void AcceptTerms()
        {
            AppSettings.AddOrUpdateValue("Terms_" + VersionTracking.CurrentVersion, "User Accepted");
            await _navigationService.NavigateAsync("SdctMasterDetailPage/NavigationPage/RootPage");
        }
    }
}
