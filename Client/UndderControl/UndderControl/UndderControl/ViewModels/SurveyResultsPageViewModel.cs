using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UndderControl.Custom;
using UndderControl.Events;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControl.Text;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class SurveyResultsPageViewModel : ViewModelBase
    {
        private SurveyResponseDto _response;
        
        private IDictionary<string, List<string>> statements;
        public IDictionary<string, List<string>> Statements
        {
            get { return statements; }
            set
            {
                statements = value;
                RaisePropertyChanged();
                _eventAggregator.GetEvent<StatementsUpdatedEvent>().Publish();
            }
        }
        private ObservableCollection<ChartDataModel> radarData;
        public ObservableCollection<ChartDataModel> RadarData {
            get { return radarData; }
            set {
                radarData = value;
                RaisePropertyChanged();
            } 
        }
        private string suitabilityStatement;
        public string SuitabilityStatement {
            get { return suitabilityStatement; }
            set {
                suitabilityStatement = value;
                RaisePropertyChanged();
            }
        }
        private string radarColour;
        public string RadarColour {
            get { return radarColour; }
            set {
                radarColour = value;
                RaisePropertyChanged();
            }
        }
        private string assessmentDate;
        public string AssessmentDate
        {
            get { return assessmentDate; }
            set
            {
                assessmentDate = value;
                RaisePropertyChanged();
            }
        }
        private DelegateCommand _compareCommand;
        public DelegateCommand CompareCommand => _compareCommand ?? (_compareCommand = new DelegateCommand(NavigateAsync, CanCompareNavigate));

        private readonly IEventAggregator _eventAggregator;

        public SurveyResultsPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager, IEventAggregator eventAggregator)
            : base(navigationService, metricsManager)
        {
            _eventAggregator = eventAggregator;
            Title = AppTextResource.SurveyResultsPageTitle;
            BuildResultData();
        }

        async Task GetSurveyResults()
        {
            var serviceResponse = await ApiManager.GetResponseByFarmId(App.SelectedFarm.ID);
            if (serviceResponse.IsSuccessStatusCode || serviceResponse.StatusCode == HttpStatusCode.NotModified)
            {
                try
                {
                    var response = await serviceResponse.Content.ReadAsStringAsync();
                    var responseData = await Task.Run(() => JsonConvert.DeserializeObject<List<SurveyResponseDto>>(response));
                    if (responseData != null && responseData.Count == 1)
                    {
                        App.LatestSurveyResponse = responseData[0];
                    }
                    else if (responseData != null && responseData.Count == 2)
                    {
                        App.LatestSurveyResponse = responseData[0];
                        App.PreviousSurveyResponse = responseData[1];
                        CompareCommand.RaiseCanExecuteChanged();
                    }
                }
                catch (Exception ex)
                {
                    MetricsManager.TrackException("Error reading survey json", ex);
                }
            }
        }

        private async void BuildResultData()
        {
            PageDialog.ShowLoading("Loading");

            if (App.LatestSurveyResponse != null)
            {
                _response = App.LatestSurveyResponse;
            }
            else
            {
                try
                {
                    await RunSafe(GetSurveyResults());
                    _response = App.LatestSurveyResponse;
                }
                catch (Exception ex)
                {
                    MetricsManager.TrackException(ex.Message, ex);
                }
            }

            if (_response != null)
            {
                var chartData = new List<ChartDataModel>();
                var answers = _response.QuestionResponses;
                var statements = new Dictionary<string, List<string>>();
                var isSuitable = true;
                var lowestScore = 5;

                foreach (var stage in App.LatestSurvey.Stages)
                {
                    //Hardcoded skip of stage 1, this could be a parameter in the stage model instead? 
                    if (stage.ID > 1)
                    {
                        try
                        {
                            //Add Stage to spidergraph
                            var score = answers.Where(a => a.StageID == stage.ID && a.QuestionResponse == true).Count();
                            chartData.Add(new ChartDataModel(stage.StageText, score));

                            if (score <= 2)
                                isSuitable = false;

                            if (score < lowestScore)
                                lowestScore = score;

                            //Add any statements from questions answered 'No'
                            if (answers.Where(a => a.StageID == stage.ID && a.QuestionResponse == false).Count() > 0)
                            {
                                var stageStatements = new List<string>();
                                foreach (var answer in answers.Where(a => a.StageID == stage.ID && a.QuestionResponse == false))
                                {
                                    stageStatements.Add(answer.QuestionStatement);
                                }
                                statements.Add(stage.StageText, stageStatements);
                            }
                        }
                        catch (Exception ex)
                        {
                            MetricsManager.TrackException("Failed to build summary data", ex);
                        }
                    }
                }

                RadarData = new ObservableCollection<ChartDataModel>(chartData);
                Statements = new ObservableDictionary<string, List<string>>(statements);
                if (isSuitable)
                    SuitabilityStatement = AppTextResource.SurveyResultSdctSuitable;
                else
                    SuitabilityStatement = AppTextResource.SurveyResultSdctUnsuitable;

                RadarColour = ReturnHexValue(lowestScore);
                AssessmentDate = _response.SubmittedDate.ToShortDateString();
            }

            PageDialog.HideLoading();
        }
        private string ReturnHexValue(int score)
        {
            string hexValue;
            switch (score)
            {
                case 1:
                    hexValue = "#F9AC95";
                    break;
                case 2:
                    hexValue = "#F9AC95";
                    break;
                case 3:
                    hexValue = "#FFDBAA";
                    break;
                case 4:
                    hexValue = "#D3E7AF";
                    break;
                case 5:
                    hexValue = "#D3E7AF";
                    break;
                default:
                    hexValue = "#030303";
                    break;
            }
            return hexValue;
        }

        async void NavigateAsync()
        {
            if (App.PreviousSurveyResponse != null)
            {
                await NavigationService.NavigateAsync("SurveyComparisonPage");
            }
            else
            {
                await NavigationService.NavigateAsync("NoResultsComparisonPage");
            }
        }
        private bool CanCompareNavigate()
        {
            return App.PreviousSurveyResponse == null ? false : true;
        }
    }
}
