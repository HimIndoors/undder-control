using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Text;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class CowStatusInputPageViewModel : ViewModelBase, IInitialize
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _onNextCommand;
        private DelegateCommand _onFinishCommand;
        public DelegateCommand OnNextCommand => _onNextCommand ?? (_onNextCommand = new DelegateCommand(NextInput));
        public DelegateCommand OnFinishCommand => _onFinishCommand ?? (_onFinishCommand = new DelegateCommand(FinishInput));

        private string _inputMode;
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
        private string _inputModeText;
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
        private InfectedType _infectedValue;
        public InfectedType InfectedValue
        {
            get { return _infectedValue; }
            set
            {
                SetProperty(ref _infectedValue, value);
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<InfectedType> _valueList;
        public ObservableCollection<InfectedType> ValueList
        {
            get { return _valueList; }
            set
            {
                _valueList = value;
                RaisePropertyChanged("ValueList");
            }
        }

        public CowStatusInputPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = AppResource.CowStatusPageTitle;
            IsBusy = true;

            ValueList = new ObservableCollection<InfectedType>
            {
                new InfectedType { Id=1, Name="Infected" },
                new InfectedType { Id=2, Name="Not infected" }
            };
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
                await _navigationService.NavigateAsync(new Uri("SdctMasterDetailPage/CowStatusFinishPage", UriKind.Absolute));
            }
            else
            {
                await _navigationService.NavigateAsync(new Uri("NavigationPage/CowStatusResultsPage", UriKind.Relative));
            }
        }

        private async Task UploadCowStatus(CowStatusDto status)
        {
            var response = await ApiManager.CreateCowStatus(status);

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

    public class InfectedType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Override string and return what you want to be displayed
        public override string ToString() => Name;
    }
}
