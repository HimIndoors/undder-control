using Prism.Commands;
using Prism.Navigation;
using UndderControl.Text;

namespace UndderControl.ViewModels
{
    public class CowStatusPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand<string> _onNavigateCommand;

        public DelegateCommand<string> OnNavigateCommand 
            => _onNavigateCommand ?? (_onNavigateCommand = new DelegateCommand<string>(NavigateAsync));
        public CowStatusPageViewModel(INavigationService navigationService)
            :base (navigationService)
        {
            Title = AppResource.CowStatusPageTitle;
            _navigationService = navigationService;
        }

        private async void NavigateAsync(string mode)
        {
            var navigationParams = new NavigationParameters
            {
                { "mode", mode }
            };
            await _navigationService.NavigateAsync("CowStatusInputPage", navigationParams);
        }
    }
}
