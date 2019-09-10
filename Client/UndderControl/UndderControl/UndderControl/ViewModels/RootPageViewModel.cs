﻿using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Services;
using UndderControlLib.Dtos;
using Xamarin.Forms;

namespace UndderControl.ViewModels
{
    public class RootPageViewModel : ViewModelBase
    {
        private ObservableCollection<FarmDto> _farmList;
        public ObservableCollection<FarmDto> FarmList
        {
            get { return _farmList; }
            set
            {
                _farmList = value;
                RaisePropertyChanged("FarmList");
            }
        }
        private FarmDto _selectedFarm { get; set; }
        public FarmDto SelectedFarm
        {
            get { return _selectedFarm; }
            set
            {
                _selectedFarm  = value;
                App.SelectedFarm = value; //Update global farm
                OnNavigateCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> OnNavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(NavigateAsync, CanNavigate));

        public RootPageViewModel(INavigationService navigationService, IMetricsManagerService metricsSevice) 
            : base(navigationService, metricsSevice)
        {
            Title = "Undder Control";
            if (App.SelectedFarm != null) SelectedFarm = App.SelectedFarm;
            InitAsync();
        }

        async void InitAsync()
        {
            try
            {
                await RunSafe(GetFarms());
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException("GetFarmsFailed", ex);
            }
        }

        async Task GetFarms()
        {
            var userId = 1;
            var farmresponse = await ApiManager.GetFarmsByUserId(userId);

            if (farmresponse.IsSuccessStatusCode)
            {
                var response = await farmresponse.Content.ReadAsStringAsync();
                var json = await Task.Run(() => JsonConvert.DeserializeObject<List<FarmDto>>(response));
                FarmList = new ObservableCollection<FarmDto>(json);
            }
            else
            {
                await PageDialog.AlertAsync("Unable to retrieve farm data", "Error", "OK");
            }  
        }

        async void NavigateAsync(string page)
        {
            MetricsManager.TrackEvent("Navigate: " + page);
            await NavigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }

        bool CanNavigate(string obj)
        {
            return App.SelectedFarm == null ? false : true;
        }
    }
}
