using Microcharts;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Collections;
using UndderControl.Events;
using UndderControl.Services;
using UndderControlLib.Dtos;
using Entry = Microcharts.Entry;

namespace UndderControl.ViewModels
{
    public class SurveyResultsPageViewModel : ViewModelBase, IInitialize
    {
        private SurveyResponseDto _response;
        private readonly IEventAggregator _eventAggregator;
        private DelegateCommand _compareCommand;
        private List<SurveyResponseDto> _responses;
        public IList<Entry> Results;
        public IDictionary<string, List<string>> Statements;
        public RadarChart Chart;
        public DelegateCommand CompareCommand => _compareCommand ?? (_compareCommand = new DelegateCommand(NavigateAsync));

        public SurveyResultsPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            _eventAggregator = eventAggregator;
            Init();
        }
        private void Init()
        {
            PageDialog.ShowLoading("Loading");
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

        public void Initialize(INavigationParameters parameters)
        {
            _response = parameters["response"] as SurveyResponseDto;
            Results = new ObservableCollection<Entry>();
            Statements = new ObservableDictionary<string, List<string>>();
            if (_response != null)
            {

                var answers = _response.QuestionResponses;

                foreach (var stage in App.LatestSurvey.Stages)
                {
                    //Hardcoded skip of stage 0, this could be a parameter in the stage model instead? 
                    if (stage.ID > 0)
                    {
                        //Add Stage to spidergraph
                        var score = answers.Where(a => a.StageID == stage.ID && a.QuestionResponse == true).Count();
                        Results.Add(
                            new Entry(score)
                            {
                                Label = stage.StageText,
                                ValueLabel = score.ToString(),
                                Color = SKColor.Parse(ReturnHexValue(score)),
                            });

                        //Add any statements from questions answered 'No'
                        if (answers.Where(a => a.StageID == stage.ID && a.QuestionResponse==false).Count() > 0)
                        {
                            var statements = new List<string>();
                            foreach (var answer in answers.Where(a => a.StageID == stage.ID && a.QuestionResponse == false))
                            {
                                statements.Add(answer.QuestionStatement);
                            }
                            Statements.Add(stage.StageText, statements);
                        }
                    }
                }
            }
            _eventAggregator.GetEvent<SurveyResultsEvent>().Publish();
            PageDialog.HideLoading();
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
