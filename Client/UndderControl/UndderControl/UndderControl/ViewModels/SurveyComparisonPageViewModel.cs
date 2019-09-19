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
    public class SurveyComparisonPageViewModel : ViewModelBase
    {
        public ObservableCollection<ChartDataModel> RadarData1 { get; set; }
        public ObservableCollection<ChartDataModel> RadarData2 { get; set; }
        public ObservableDictionary<string,List<string>> Improvements { get; set; }
        public string RadarColour1 { get; set; }
        public string RadarColour2 { get; set; }
        public string RadarLabel1 { get; set; }
        public string RadarLabel2 { get; set; }
        public string ImprovementStatement { get; set; }

        public SurveyComparisonPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager)
            : base(navigationService, metricsManager)
        {
            Title = AppTextResource.TitleResultComparison;
            Init();
        }

        private void Init()
        {
            RadarData1 = new ObservableCollection<ChartDataModel>(GenerateRadarData(App.LatestSurveyResponse.QuestionResponses));
            RadarData2 = new ObservableCollection<ChartDataModel>(GenerateRadarData(App.PreviousSurveyResponse.QuestionResponses));
            RadarColour1 = ReturnHexValue((int)RadarData1.OrderByDescending(x => x.Value).First().Value);
            RadarColour2 = ReturnHexValue((int)RadarData2.OrderByDescending(x => x.Value).First().Value);
            RadarLabel1 = App.LatestSurveyResponse.SubmittedDate.Year.ToString();
            RadarLabel2 = App.PreviousSurveyResponse.SubmittedDate.Year.ToString();
            GenerateImprovementData();
        }

        private void GenerateImprovementData()
        {
            var improvements = new Dictionary<string, List<string>>
            {
                { AppTextResource.AreasWhichHaveImproved, new List<string>()},
                { AppTextResource.AreasWhichHaveImprovedButStillNeedImprovement, new List<string>()},
                { AppTextResource.AreasWhichRemainedStableButStillNeedImprovement, new List<string>()},
                { AppTextResource.AreasWhichNeedImprovement, new List<string>()}
            };

            var isImprovement = false;
            for (var i= 0; i < 5  ; i++)
            {
                var latestScore = RadarData1[i].Value; 
                var previousScore = RadarData2[i].Value; 
                var name = RadarData1[i].Name;

                  
                if (latestScore > previousScore && latestScore > 3)
                {
                    improvements[AppTextResource.AreasWhichHaveImproved].Add(name);
                    isImprovement = true;
                } 
                else if(latestScore > previousScore && latestScore < 4)
                {
                    improvements[AppTextResource.AreasWhichHaveImprovedButStillNeedImprovement].Add(name);
                    isImprovement = true;
                }  
                else if (latestScore == previousScore)
                {
                    improvements[AppTextResource.AreasWhichRemainedStableButStillNeedImprovement].Add(name);
                }
                else if (latestScore < 3)
                {
                    improvements[AppTextResource.AreasWhichNeedImprovement].Add(name);
                }
            }

            Improvements = new ObservableDictionary<string, List<string>>(improvements);
            ImprovementStatement = isImprovement ? AppTextResource.ImprovementPositive : AppTextResource.ImprovementNevagive;
        }
        private List<ChartDataModel> GenerateRadarData(IList<SurveyQuestionResponseDto> answers)
        {
            var chartData = new List<ChartDataModel>();
            foreach (var stage in App.LatestSurvey.Stages)
            {
                //Hardcoded skip of stage 1, this could be a parameter in the stage model instead? 
                if (stage.ID > 1)
                {
                    try
                    {
                        var score = answers.Where(a => a.StageID == stage.ID && a.QuestionResponse == true).Count();
                        chartData.Add(new ChartDataModel(stage.StageText, score));
                    }
                    catch (Exception ex)
                    {
                        MetricsManager.TrackException("Failed to build summary data", ex);
                    }
                }
            }
            return chartData;
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
    }
}
