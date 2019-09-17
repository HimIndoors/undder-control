using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UndderControl.Custom;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControl.Text;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class CowStatusComparisonPageViewModel : ViewModelBase
    {
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

        private readonly ObservableDictionary<string, int> latestResults;
        private readonly ObservableDictionary<string, int> previousResults;

        private string _resultYear;
        public string ResultYear
        {
            get { return _resultYear; }
            set
            {
                _resultYear = value;
                RaisePropertyChanged("ResultYear");
            }
        }

        public CowStatusComparisonPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            Title = "";
            latestResults = BuildCowData(App.LatestCowStatusData);
            previousResults = BuildCowData(App.PreviousCowStatusData);
            BuildGraphData();
        }

        private void BuildGraphData()
        {
            var latestYear = App.LatestCowStatusData[0].DateAddedCalving.Value.Year.ToString();
            var previousYear = App.PreviousCowStatusData[0].DateAddedCalving.Value.Year.ToString();

            NiRateHealthy = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(latestYear, (int)Math.Round((double)(100 * latestResults[AppResource.CsPreventionOfNewInfection]) / latestResults[AppResource.CsNotInfectedAtDryoff])),
                new ChartDataModel(previousYear, (int)Math.Round((double)(100 * previousResults[AppResource.CsPreventionOfNewInfection]) / previousResults[AppResource.CsNotInfectedAtDryoff]))
            };
            NiRateNewInfection = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(latestYear, (int)Math.Round((double)(100 * latestResults[AppResource.CsNewInfection]) / latestResults[AppResource.CsNotInfectedAtDryoff])),
                new ChartDataModel(previousYear, (int)Math.Round((double)(100 * previousResults[AppResource.CsNewInfection]) / previousResults[AppResource.CsNotInfectedAtDryoff]))
            };
            CureRateHealthy = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(latestYear, (int)Math.Round((double)(100 * latestResults[AppResource.CsCure]) / latestResults[AppResource.CsInfectedAtDryoff])),
                new ChartDataModel(previousYear, (int)Math.Round((double)(100 * previousResults[AppResource.CsCure]) / previousResults[AppResource.CsInfectedAtDryoff]))
            };
            CureRateInfected = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(latestYear, (int)Math.Round((double)(100 * latestResults[AppResource.CsFailureToCure]) / latestResults[AppResource.CsInfectedAtDryoff])),
                new ChartDataModel(previousYear, (int)Math.Round((double)(100 * previousResults[AppResource.CsFailureToCure]) / previousResults[AppResource.CsInfectedAtDryoff])),
            };

            ResultYear = previousYear + " / " + latestYear;
        }

        private ObservableDictionary<string, int> BuildCowData(List<CowStatusDto> statusList)
        {
            //Build results dict with zero values
            var resultSet = new ObservableDictionary<string, int>
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

            foreach (CowStatusDto cs in statusList)
            {
                if (cs.InfectedAtDryOff && cs.InfectedAtCalving)
                {
                    resultSet[AppResource.CsInfectedAtDryoff]++;
                    resultSet[AppResource.CsInfectedAfterCalving]++;
                    resultSet[AppResource.CsFailureToCure]++;
                }
                else if (cs.InfectedAtDryOff && !cs.InfectedAtCalving)
                {
                    resultSet[AppResource.CsInfectedAtDryoff]++;
                    resultSet[AppResource.CsNotInfectedAfterCalving]++;
                    resultSet[AppResource.CsCure]++;
                }
                else if (!cs.InfectedAtDryOff && cs.InfectedAtCalving)
                {
                    resultSet[AppResource.CsNotInfectedAtDryoff]++;
                    resultSet[AppResource.CsInfectedAfterCalving]++;
                    resultSet[AppResource.CsNewInfection]++;
                }
                else if (!cs.InfectedAtDryOff && !cs.InfectedAtCalving)
                {
                    resultSet[AppResource.CsNotInfectedAtDryoff]++;
                    resultSet[AppResource.CsNotInfectedAfterCalving]++;
                    resultSet[AppResource.CsPreventionOfNewInfection]++;
                }
            }

            return resultSet;
        }
    }
}
