using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System.Collections.ObjectModel;
using UndderControl.Events;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControl.Views;

namespace UndderControl.ViewModels
{
    public class SdctMasterDetailPageViewModel : ViewModelBase
    {
        private readonly IEventAggregator EventAggregator;
        private ObservableCollection<MenuItemModel> menuItems;
        public ObservableCollection<MenuItemModel> MenuItems
        {
            get { return menuItems; }
            set {
                menuItems = value;
                RaisePropertyChanged("MenuItems");
            }
        }

        public DelegateCommand<MenuItemModel> OnItemTapped { get; set; }

        public SdctMasterDetailPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager, IEventAggregator eventAggregator)
            : base(navigationService, metricsManager)
        {
            Title = "UnDDER CONTROL";
            EventAggregator = eventAggregator;
            OnItemTapped = new DelegateCommand<MenuItemModel>(MenuNavigate);

            MenuItems = new ObservableCollection<MenuItemModel>
            {
                new MenuItemModel{ Name="Home", Icon="home.png",Page="/SdctMasterDetailPage/NavigationPage/RootPage"},
                new MenuItemModel{ Name="Manage Farms", Icon="farm.png",Page="NavigationPage/ManageFarmsPage"},
                new MenuItemModel{ Name="Log out", Icon="user.png",Page="/NavigationPage/LoginPage"}
            };
        }

        async void MenuNavigate(MenuItemModel item)
        {
            if (item != null && item.Name.Equals("Log out"))
            {
                UserSettings.UserId = -1;
                EventAggregator.GetEvent<LogOutEvent>().Publish();
            }
            var page = item.Page;
            await NavigationService.NavigateAsync(page);
        }
    }
}
