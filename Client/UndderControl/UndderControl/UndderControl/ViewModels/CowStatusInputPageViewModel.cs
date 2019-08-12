using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Text;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class CowStatusInputPageViewModel : ViewModelBase, IInitialize
    {
        INavigationService _navigationService;
        public DelegateCommand OnNextCommand { get; private set; }
        public DelegateCommand OnFinishCommand { get; private set; }

        string _inputMode;
        public string InputMode
        {
            get { return _inputMode; }
            set {
                SetProperty(ref _inputMode, value);
                if(_inputMode.Equals("dryoff"))
                {
                    InputModeText = AppResource.CowStatusInputTextDryoffStatus;
                }
                else
                {
                    InputModeText = AppResource.CowStatusInputTextCalvingStatus;
                }
            }
        }
        string _inputModeText;
        public string InputModeText
        {
            get { return _inputModeText; }
            set { SetProperty(ref _inputModeText, value); }
        }
        private CowStatusDto _cowStatus = new CowStatusDto();
        public CowStatusDto CowStatus
        {
            get { return _cowStatus; }
            private set
            {
                SetProperty(ref _cowStatus, value);
            }
        }

        public CowStatusInputPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = AppResource.CowStatusPageTitle;
            IsBusy = true;
            OnFinishCommand = new DelegateCommand(FinishInput);
            OnNextCommand = new DelegateCommand(NextInput);
        }

        private async void NextInput()
        {
            /*
            await RunSafe(UploadCowStatus(CowStatus));
            */
            CowStatus = new CowStatusDto();

        }

        private async void FinishInput()
        {
            /*
            await RunSafe(UploadCowStatus(CowStatus));
            */
            if (InputMode.Equals("dryoff"))
            {
                await _navigationService.NavigateAsync(new Uri("NavigationPage/CowStatusFinishPage", UriKind.Relative));
            }
            else
            {
                await _navigationService.NavigateAsync(new Uri("NavigationPage/CowStatusResultsPage", UriKind.Relative));
            }
        }

        private async Task UploadCowStatus(CowStatusDto status)
        {
            var response = await ApiManager.UploadCowStatus(status);

            if (!response.IsSuccessStatusCode)
            {
                await PageDialog.AlertAsync("Unable to save cow status data", "Error", "OK");
            }
        }

        public void Initialize(INavigationParameters parameters)
        {
            if(parameters.ContainsKey("mode"))
            {
                InputMode = parameters["mode"] as string;
            }
            IsBusy = false;
        }
    }
}
