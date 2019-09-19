using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UndderControl.Helpers;
using UndderControl.Services;

namespace UndderControl.ViewModels
{
    public class SdctMasterDetailPageViewModel : ViewModelBase
    {
        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> OnNavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(NavigateAsync));

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
            Title = "UNDDER CONTROL";
            OnItemTapped = new DelegateCommand<MenuItemModel>(MenuNavigate);
            MenuItems = new ObservableCollection<MenuItemModel>
            {
                new MenuItemModel{ Name="Home", Icon="home.png",Page="/SdctMasterDetailPage/NavigationPage/RootPage"},
                new MenuItemModel{ Name="Manage Farms", Icon="farm.png",Page="ManageFarmsPage"},
                new MenuItemModel{ Name="About", Icon="about.png",Page="AboutPage"},
            };
        }

        async void NavigateAsync(string page)
        {
            await NavigationService.NavigateAsync(page);
        }

        async void MenuNavigate(MenuItemModel item)
        {
            var page = item.Page;
            await NavigationService.NavigateAsync(page);
        }
    }
}
