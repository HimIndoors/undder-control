using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UndderControl.Services;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class MonitorPageViewModel : ViewModelBase
    {
        private DelegateCommand<string> _onStatusCommand;
        public DelegateCommand<string> OnStatusCommand
            => _onStatusCommand ?? (_onStatusCommand = new DelegateCommand<string>(NavigateAsync));

        private DelegateCommand<string> _onSummaryCommand;
        public DelegateCommand<string> OnSummaryCommand
            => _onSummaryCommand ?? (_onSummaryCommand = new DelegateCommand<string>(NavigateAsync, CanSummaryNavigate));

        private DelegateCommand<string> _onCompareCommand;
        public DelegateCommand<string> OnCompareCommand
            => _onCompareCommand ?? (_onCompareCommand = new DelegateCommand<string>(NavigateAsync, CanCompareNavigate));

        public MonitorPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            Title = "";
            PopulateCowStatusData();
        }

        private async void PopulateCowStatusData()
        {
            try
            {
                await RunSafe(GetCowStatusData());
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException(ex.Message, ex);
            }
        }

        async Task GetCowStatusData()
        {
            var tryHistoric = false;
            var serviceResponse = await ApiManager.GetCowsStatusByFarmID(App.SelectedFarm.ID);

            if (serviceResponse.IsSuccessStatusCode || serviceResponse.StatusCode == HttpStatusCode.NotModified)
            {
                var response = await serviceResponse.Content.ReadAsStringAsync();
                var cowStatusData = await Task.Run(() => JsonConvert.DeserializeObject<List<CowStatusDto>>(response));
                if (cowStatusData != null && cowStatusData.Count > 0)
                {
                    App.LatestCowStatusData = new List<CowStatusDto>(cowStatusData);
                    OnSummaryCommand.RaiseCanExecuteChanged();
                    tryHistoric = true;
                }   
            }

            if (tryHistoric)
            {
                var previousYear = App.LatestCowStatusData[0].DateAddedCalving.Value.Year - 1;
                var historicResponse = await ApiManager.GetCowsStatusByFarmIDandYear(App.SelectedFarm.ID, previousYear);

                if (historicResponse.IsSuccessStatusCode)
                {
                    var response = await historicResponse.Content.ReadAsStringAsync();
                    var cowStatusData = await Task.Run(() => JsonConvert.DeserializeObject<List<CowStatusDto>>(response));
                    if (cowStatusData != null && cowStatusData.Count > 0)
                    {
                        App.PreviousCowStatusData = new List<CowStatusDto>(cowStatusData);
                        OnCompareCommand.RaiseCanExecuteChanged();
                    }  
                }
            }
            
        }

        private async void NavigateAsync(string page)
        {
            await NavigationService.NavigateAsync(new Uri(page, UriKind.Relative));
        }

        bool CanSummaryNavigate(string obj)
        {
            return App.LatestCowStatusData == null ? false : true;
        }

        bool CanCompareNavigate(string obj)
        {
            return (App.LatestCowStatusData == null ? false : true) && (App.PreviousCowStatusData == null ? false : true);
        }
    }
}
