using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Custom;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControl.Text;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class SurveyResultsPageViewModel : ViewModelBase
    {
        private readonly SurveyResponseDto _response;
        private DelegateCommand _compareCommand;
        private List<SurveyResponseDto> _responses;
        public IDictionary<string, List<string>> Statements;
        public ObservableCollection<ChartDataModel> RadarData { get; set; }
        public DelegateCommand CompareCommand => _compareCommand ?? (_compareCommand = new DelegateCommand(NavigateAsync));
        private string _suitabilityStatement;
        public string SuitabilityStatement { get; set; }
        public string RadarColour { get; set; }

        public SurveyResultsPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            _response = App.LatestSurveyResponse;
            BuildResultData();
        }
        private void BuildResultData()
        {
            PageDialog.ShowLoading("Loading");

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
            try
            {
                await RunSafe(GetResponses());
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException("GetSurveyResponsesFailed", ex);
            }

            if (_responses != null && _responses.Count > 1)
            {
                var navigationParams = new NavigationParameters
                {
                    { "responses", _responses }
                };
                await NavigationService.NavigateAsync("SurveyComparisonPage", navigationParams);
            }
            else
            {
                await NavigationService.NavigateAsync("/SdctMasterDetailPage/NavigationPage/NoResultComparisonPage");
            }
        }

        async Task GetResponses()
        {
            var response = await ApiManager.GetResponseByFarmId(App.SelectedFarm.ID);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var json = await Task.Run(() => JsonConvert.DeserializeObject<List<SurveyResponseDto>>(responseString));
                _responses = json;
            }
            else
            {
                await PageDialog.AlertAsync("Unable to retrieve survey response data", "Error", "OK");
            }
        }
    }
}
