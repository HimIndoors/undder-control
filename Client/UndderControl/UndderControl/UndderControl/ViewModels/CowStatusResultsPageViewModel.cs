using Newtonsoft.Json;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UndderControl.Collections;
using UndderControl.Events;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControl.Text;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class CowStatusResultsPageViewModel : ViewModelBase
    {
        private List<CowStatusDto> _cowStatusList;
        private ObservableDictionary<string, int> _results;
        public ObservableDictionary<string, int> Results
        {
            get { return _results; }
            set
            {
                SetProperty(ref _results, value);
                RaisePropertyChanged("Results");
            }
        }
        private readonly IEventAggregator _eventAggregator;

        #region Graph Data
        private ObservableCollection<ChartDataModel> _niRateHealthy;
        public ObservableCollection<ChartDataModel> NiRateHealthy
        {
            get { return _niRateHealthy; }
            set
            {
                SetProperty(ref _niRateHealthy, value);
                RaisePropertyChanged("NiRateHealthy");
            }
        }
        private ObservableCollection<ChartDataModel> _niRateNewInfection;
        public ObservableCollection<ChartDataModel> NiRateNewInfection
        {
            get { return _niRateNewInfection; }
            set
            {
                SetProperty(ref _niRateNewInfection, value);
                RaisePropertyChanged("NiRateNewInfection");
            }
        }
        private ObservableCollection<ChartDataModel> _cureRateHealthy;
        public ObservableCollection<ChartDataModel> CureRateHealthy
        {
            get { return _cureRateHealthy; }
            set
            {
                SetProperty(ref _cureRateHealthy, value);
                RaisePropertyChanged("CureRateHealthy");
            }
        }
        private ObservableCollection<ChartDataModel> _cureRateInfected;
        public ObservableCollection<ChartDataModel> CureRateInfected
        {
            get { return _cureRateInfected; }
            set
            {
                SetProperty(ref _cureRateInfected, value);
                RaisePropertyChanged("CureRateInfected");
            }
        }
        #endregion Graph Data

        public CowStatusResultsPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager, IEventAggregator ea)
            : base(navigationService, metricsManager)
        {
            Title = "";
            _eventAggregator = ea;
            InitAsync();
        }

        private async void InitAsync()
        {
            try
            {
                await RunSafe(GetCowStatus());
                BuildCowData();
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException("GetFarmsFailed", ex);
            }


        }

        private void BuildCowData()
        {
            //Build results dict with zero values
            var temp = new ObservableDictionary<string, int>
            {
                { AppResource.CsNotInfectedAtDryoff, 0 },
                { AppResource.CsInfectedAtDryoff, 0 },
                { AppResource.CsNotInfectedAfterCalving, 0 },
                { AppResource.CsInfectedAfterCalving, 0 },
                { AppResource.CsNewInfection, 0 },
                { AppResource.CsPreventionOfNewInfection, 0 },
                { AppResource.CsFailureToCure, 0 },
                { AppResource.CsCure, 0 },
            };

            foreach (CowStatusDto cs in _cowStatusList)
            {
                if (cs.InfectedAtDryOff && cs.InfectedAtCalving)
                {
                    temp[AppResource.CsInfectedAtDryoff]++;
                    temp[AppResource.CsInfectedAfterCalving]++;
                    temp[AppResource.CsFailureToCure]++;
                }
                else if(cs.InfectedAtDryOff && !cs.InfectedAtCalving)
                {
                    temp[AppResource.CsInfectedAtDryoff]++;
                    temp[AppResource.CsNotInfectedAfterCalving]++;
                    temp[AppResource.CsCure]++;
                }
                else if (!cs.InfectedAtDryOff && cs.InfectedAtCalving)
                {
                    temp[AppResource.CsNotInfectedAtDryoff]++;
                    temp[AppResource.CsInfectedAfterCalving]++;
                    temp[AppResource.CsNewInfection]++;
                }
                else if (!cs.InfectedAtDryOff && !cs.InfectedAtCalving)
                {
                    temp[AppResource.CsNotInfectedAtDryoff]++;
                    temp[AppResource.CsNotInfectedAfterCalving]++;
                    temp[AppResource.CsPreventionOfNewInfection]++;
                }
            }
            Results = temp;
            _eventAggregator.GetEvent<CowStatusResultsEvent>().Publish();

            //Set up graph data
            NiRateHealthy = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Current Rate", (int)Math.Round((double)(100 * Results[AppResource.CsPreventionOfNewInfection]) / Results[AppResource.CsNotInfectedAtDryoff]))
            };
            NiRateNewInfection = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Current Rate", (int)Math.Round((double)(100 * Results[AppResource.CsNewInfection]) / Results[AppResource.CsNotInfectedAtDryoff])),
            };
            CureRateHealthy = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Current Rate", (int)Math.Round((double)(100 * Results[AppResource.CsCure]) / Results[AppResource.CsInfectedAtDryoff])),
            };
            CureRateInfected = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Current Rate", (int)Math.Round((double)(100 * Results[AppResource.CsFailureToCure]) / Results[AppResource.CsInfectedAtDryoff])),
            };
        }

        private async Task GetCowStatus()
        {
            try
            {
                var apiresponse = await ApiManager.GetStatusByFarmId(App.SelectedFarm.ID);
                if (apiresponse.IsSuccessStatusCode)
                {
                    var response = await apiresponse.Content.ReadAsStringAsync();
                    var json = await Task.Run(() => JsonConvert.DeserializeObject<List<CowStatusDto>>(response));
                    _cowStatusList = json;
                }
                else
                {
                    await PageDialog.AlertAsync("Unable to retrieve cow status data", "Error", "OK");
                }
            }
            catch(Exception ex)
            {
                MetricsManager.TrackException("Error getting cowstatus data", ex);
                await PageDialog.AlertAsync("Unable to retrieve cow status data", "Error", "OK");
            }

            
        }
    }
}
