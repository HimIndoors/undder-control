using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using UndderControl.Helpers;
using UndderControl.Services;

namespace UndderControl.ViewModels
{
    public class SdctMasterDetailPageViewModel : ViewModelBase
    {
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

        public SdctMasterDetailPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            Title = "UnDDER CONTROL";
            OnItemTapped = new DelegateCommand<MenuItemModel>(MenuNavigate);

            MenuItems = new ObservableCollection<MenuItemModel>
            {
                new MenuItemModel{ Name="Home", Icon="home.png",Page="/SdctMasterDetailPage/NavigationPage/RootPage"},
                new MenuItemModel{ Name="Manage Farms", Icon="farm.png",Page="NavigationPage/ManageFarmsPage"},
                //new MenuItemModel{ Name="Assess Farm", Icon="farm.png",Page="NavigationPage/AssessmentPage"},
                //new MenuItemModel{ Name="Monitor Farm", Icon="farm.png",Page="NavigationPage/MonitorPage"},
                //new MenuItemModel{ Name="About", Icon="about.png",Page="NavigationPage/AboutPage"}
            };
        }

        async void MenuNavigate(MenuItemModel item)
        {
            var page = item.Page;
            await NavigationService.NavigateAsync(page);
        }
    }
}
