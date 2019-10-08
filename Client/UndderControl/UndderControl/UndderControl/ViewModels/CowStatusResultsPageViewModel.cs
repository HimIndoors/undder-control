﻿using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UndderControl.Custom;
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

        private int _resultYear;
        public int ResultYear
        {
            get { return _resultYear; }
            set {
                _resultYear = value;
                RaisePropertyChanged("ResultYear");
            }
        }

        private int _niThreshold;
        public int NiThreshold
        {
            get { return _niThreshold; }
            set
            {
                _niThreshold = value;
                PreventThreshold = 100 - value;
                RaisePropertyChanged("NiThreshold");
            }
        }

        private int _cureThreshold;
        public int CureThreshold
        {
            get { return _cureThreshold; }
            set
            {
                _cureThreshold = value;
                FailCureThreshold = 100 - value;
                RaisePropertyChanged("CureThreshold");
            }
        }

        private int _preventThreshold;
        public int PreventThreshold
        {
            get { return _preventThreshold; }
            set
            {
                _preventThreshold = value;
                RaisePropertyChanged("PreventThreshold");
            }
        }

        private int _failCureThreshold;
        public int FailCureThreshold
        {
            get { return _failCureThreshold; }
            set
            {
                _failCureThreshold = value;
                RaisePropertyChanged("FailCureThreshold");
            }
        }

        private DelegateCommand _compareCommand;
        public DelegateCommand CompareCommand => _compareCommand ?? (_compareCommand = new DelegateCommand(NavigateAsync));

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

        public CowStatusResultsPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            Title = AppTextResource.SummaryResultTitle;
            NiThreshold = 10;
            CureThreshold = 85;
            Init();
        }

        private void Init()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                _cowStatusList = App.LatestCowStatusData;
                BuildCowData();              
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException("Build CowStatus data failed", ex);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BuildCowData()
        {
            //Build results dict with zero values
            var temp = new ObservableDictionary<string, int>
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

            foreach (CowStatusDto cs in _cowStatusList)
            {
                if (cs.InfectedAtDryOff && cs.InfectedAtCalving)
                {
                    temp[AppTextResource.CsInfectedAtDryoff]++;
                    temp[AppTextResource.CsInfectedAfterCalving]++;
                    temp[AppTextResource.CsFailureToCure]++;
                }
                else if(cs.InfectedAtDryOff && !cs.InfectedAtCalving)
                {
                    temp[AppTextResource.CsInfectedAtDryoff]++;
                    temp[AppTextResource.CsNotInfectedAfterCalving]++;
                    temp[AppTextResource.CsCure]++;
                }
                else if (!cs.InfectedAtDryOff && cs.InfectedAtCalving)
                {
                    temp[AppTextResource.CsNotInfectedAtDryoff]++;
                    temp[AppTextResource.CsInfectedAfterCalving]++;
                    temp[AppTextResource.CsNewInfection]++;
                }
                else if (!cs.InfectedAtDryOff && !cs.InfectedAtCalving)
                {
                    temp[AppTextResource.CsNotInfectedAtDryoff]++;
                    temp[AppTextResource.CsNotInfectedAfterCalving]++;
                    temp[AppTextResource.CsPreventionOfNewInfection]++;
                }
            }
            Results = temp;

            //Set up graph data
            NiRateHealthy = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Current Rate", (int)Math.Round((double)(100 * Results[AppTextResource.CsPreventionOfNewInfection]) / Results[AppTextResource.CsNotInfectedAtDryoff]))
            };
            NiRateNewInfection = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Current Rate", (int)Math.Round((double)(100 * Results[AppTextResource.CsNewInfection]) / Results[AppTextResource.CsNotInfectedAtDryoff])),
            };
            CureRateHealthy = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Current Rate", (int)Math.Round((double)(100 * Results[AppTextResource.CsCure]) / Results[AppTextResource.CsInfectedAtDryoff])),
            };
            CureRateInfected = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Current Rate", (int)Math.Round((double)(100 * Results[AppTextResource.CsFailureToCure]) / Results[AppTextResource.CsInfectedAtDryoff])),
            };

            //Set result date
            ResultYear = _cowStatusList[0].DateAddedCalving.Value.Year;
        }

        private async void NavigateAsync()
        {
            if (App.PreviousCowStatusData != null)
            {
                await NavigationService.NavigateAsync("/SdctMasterDetailPage/NavigationPage/CowStatusComparisonPage");
            }
            else
            {
                await NavigationService.NavigateAsync("/SdctMasterDetailPage/NavigationPage/NoResultComparisonPage");
            }
        }
    }
}
