using Prism.Commands;
using Prism.Navigation;
using UndderControl.Services;
using UndderControl.Text;

namespace UndderControl.ViewModels
{
    public class CowStatusPageViewModel : ViewModelBase
    {
        private DelegateCommand<string> _onNavigateCommand;
        public DelegateCommand<string> OnNavigateCommand 
            => _onNavigateCommand ?? (_onNavigateCommand = new DelegateCommand<string>(NavigateAsync));
        public CowStatusPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            :base (navigationService, metricsManager)
        {
            Title = AppResource.CowStatusPageTitle;
        }

        private async void NavigateAsync(string mode)
        {
            var navigationParams = new NavigationParameters
            {
                { "mode", mode }
            };
            await NavigationService.NavigateAsync("CowStatusInputPage", navigationParams);
        }
    }
}
