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
        public int NiThreshold { get; set; }
        public int CureThreshold { get; set; }

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
            NiThreshold = UserSettings.NewInfectionThreshold;
            CureThreshold = UserSettings.CureThreshold;
            BuildGraphData();
        }

        private void BuildGraphData()
        {
            var latestYear = App.LatestCowStatusData[0].DateAddedCalving.Value.Year.ToString();
            var previousYear = App.PreviousCowStatusData[0].DateAddedCalving.Value.Year.ToString();

            NiRateHealthy = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(latestYear, (int)Math.Round((double)(100 * latestResults[AppTextResource.CsPreventionOfNewInfection]) / latestResults[AppTextResource.CsNotInfectedAtDryoff])),
                new ChartDataModel(previousYear, (int)Math.Round((double)(100 * previousResults[AppTextResource.CsPreventionOfNewInfection]) / previousResults[AppTextResource.CsNotInfectedAtDryoff]))
            };
            NiRateNewInfection = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(latestYear, (int)Math.Round((double)(100 * latestResults[AppTextResource.CsNewInfection]) / latestResults[AppTextResource.CsNotInfectedAtDryoff])),
                new ChartDataModel(previousYear, (int)Math.Round((double)(100 * previousResults[AppTextResource.CsNewInfection]) / previousResults[AppTextResource.CsNotInfectedAtDryoff]))
            };
            CureRateHealthy = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(latestYear, (int)Math.Round((double)(100 * latestResults[AppTextResource.CsCure]) / latestResults[AppTextResource.CsInfectedAtDryoff])),
                new ChartDataModel(previousYear, (int)Math.Round((double)(100 * previousResults[AppTextResource.CsCure]) / previousResults[AppTextResource.CsInfectedAtDryoff]))
            };
            CureRateInfected = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(latestYear, (int)Math.Round((double)(100 * latestResults[AppTextResource.CsFailureToCure]) / latestResults[AppTextResource.CsInfectedAtDryoff])),
                new ChartDataModel(previousYear, (int)Math.Round((double)(100 * previousResults[AppTextResource.CsFailureToCure]) / previousResults[AppTextResource.CsInfectedAtDryoff])),
            };

            ResultYear = previousYear + " / " + latestYear;
        }

        private ObservableDictionary<string, int> BuildCowData(List<CowStatusDto> statusList)
        {
            //Build results dict with zero values
            var resultSet = new ObservableDictionary<string, int>
            {
                { AppTextResource.CsNotInfectedAtDryoff, 0 },
                { AppTextResource.CsInfectedAtDryoff, 0 },
                { AppTextResource.CsNotInfectedAfterCalving, 0 },
                { AppTextResource.CsInfectedAfterCalving, 0 },
                { AppTextResource.CsNewInfection, 0 },
                { AppTextResource.CsPreventionOfNewInfection, 0 },
                { AppTextResource.CsFailureToCure, 0 },
                { AppTextResource.CsCure, 0 },
            };

            foreach (CowStatusDto cs in statusList)
            {
                if (cs.InfectedAtDryOff && cs.InfectedAtCalving)
                {
                    resultSet[AppTextResource.CsInfectedAtDryoff]++;
                    resultSet[AppTextResource.CsInfectedAfterCalving]++;
                    resultSet[AppTextResource.CsFailureToCure]++;
                }
                else if (cs.InfectedAtDryOff && !cs.InfectedAtCalving)
                {
                    resultSet[AppTextResource.CsInfectedAtDryoff]++;
                    resultSet[AppTextResource.CsNotInfectedAfterCalving]++;
                    resultSet[AppTextResource.CsCure]++;
                }
                else if (!cs.InfectedAtDryOff && cs.InfectedAtCalving)
                {
                    resultSet[AppTextResource.CsNotInfectedAtDryoff]++;
                    resultSet[AppTextResource.CsInfectedAfterCalving]++;
                    resultSet[AppTextResource.CsNewInfection]++;
                }
                else if (!cs.InfectedAtDryOff && !cs.InfectedAtCalving)
                {
                    resultSet[AppTextResource.CsNotInfectedAtDryoff]++;
                    resultSet[AppTextResource.CsNotInfectedAfterCalving]++;
                    resultSet[AppTextResource.CsPreventionOfNewInfection]++;
                }
            }

            return resultSet;
        }
    }
}
