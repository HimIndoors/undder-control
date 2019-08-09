using Microcharts;
using Prism.AppModel;
using Prism.Navigation;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Services;
using UndderControlLib.Dtos;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace UndderControl.ViewModels
{
    public class ResultsPageViewModel : ViewModelBase, IInitialize
    {
        SurveyResponseDto _response;
        List<Entry> _results;
        public RadarChart Chart;
        
        public ResultsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Init();
            //Chart = new RadarChart() { Entries = _results };
        }
        private void Init()
        {
            _results = new List<Entry>();
            if (_response != null){
                
                var answers = _response.QuestionResponses;

                foreach (var stage in App.LatestSurvey.Stages)
                {
                    //Hardcoded skip of stage 0, this could be a parameter in the stage model instead? 
                    if (stage.StageID > 0)
                    {
                        var score = answers.Where(a => a.StageID == stage.StageID && a.QuestionResponse == true).Count();
                        _results.Add(
                            new Entry(score)
                            {
                                Label = stage.StageTitle,
                                ValueLabel = score.ToString(),
                                Color = SKColor.Parse(ReturnHexValue(score)),
                            });
                    }
                }
            }            
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
        }
    }    
}
