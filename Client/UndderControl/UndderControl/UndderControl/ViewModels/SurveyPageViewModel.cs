using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UndderControl.Events;
using UndderControl.Extensions;
using UndderControl.Services;
using UndderControlLib.Dtos;
using Xamarin.Forms;

namespace UndderControl.ViewModels
{
    public class SurveyPageViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private SurveyResponseDto _response = SurveyResponseDtoExtensions.CreateNew();
        private int _questionIndex;
        private int _stageIndex;
        public DelegateCommand AnswerYesCommand { get; }
        public DelegateCommand AnswerNoCommand { get; }
        public DelegateCommand StartStageCommand { get; }
        public SurveyQuestionDto CurrentQuestion => Question(_questionIndex);
        public SurveyStageDto CurrentStage => Stage(_stageIndex);
        public int StageQuestionCount => QuestionCount();
        public bool ShowStage { get; private set; }
        public int QuestionIncrement { get; private set; }

        public SurveyPageViewModel(INavigationService navigationService, IMetricsManagerService metricsManager, IEventAggregator ea)
            : base(navigationService, metricsManager)
        {
            _eventAggregator = ea;
            Title = "Undder Control";
            AnswerYesCommand = new DelegateCommand(AnswerYes, () => IsNotBusy);
            AnswerNoCommand = new DelegateCommand(AnswerNo, () => IsNotBusy);
            StartStageCommand = new DelegateCommand(StartStage, () => IsNotBusy);
            Init();
        }

        private void Init()
        {
            // Work out what question we are up to and set properties accordingly 
            if (_response.QuestionResponses.Count == 0)
            {
                //Start a fresh survey
                _questionIndex = 0;
                _stageIndex = 0;
                QuestionIncrement = 1;
            }

            return;
        }

        

        private SurveyQuestionDto Question(int index)
        {
            var questions = App.LatestSurvey?.Questions;
            if (questions != null && index >= 0 && index < questions.Count)
            {
                return questions[index];
            }

            return null;
        }

        private SurveyStageDto Stage(int index)
        {
            var stages = App.LatestSurvey?.Stages;
            if (stages != null && index >= 0 && index < stages.Count)
            {
                return stages[index];
            }

            return null;
        }

        private int QuestionCount()
        {
            var stage = Stage(_stageIndex);
            var questions = App.LatestSurvey?.Questions;
            if (questions != null)
            {
                return questions.Where(q => q.Stage_ID == stage.ID).Count();
            }

            return 0;
        }

        private async void EndSurvey()
        {
            _response.SubmittedDate = DateTime.Now;

            DependencyService.Get<IMetricsManagerService>().TrackEvent("SurveyEnded");

            try
            {
                // Upload and delete the survey upon completion
                // await UploadAndDeleteAsync();

            }
            catch (Exception ex)
            {
                MetricsManager.TrackException("SurveyEndException", ex);
            }
            var navigationParams = new NavigationParameters
            {
                { "response", _response }
            };
            await NavigationService.NavigateAsync("SurveyResultsPage", navigationParams);
        }

        /// <summary>
        /// Find the index for the given question identifier.
        /// </summary>
        /// <param name="questionId">The question identifier.</param>
        /// <returns>The question index.</returns>
        private int? QuestionIndex(int? questionId)
        {
            var questions = App.LatestSurvey?.Questions;
            if (questions != null)
            {
                for (var i = 0; i < questions.Count; ++i)
                {
                    if (questions[i].ID == questionId)
                    {
                        return i;
                    }
                }
            }
            else
            {
                //We should never get here otherwise we have no questions!
                MetricsManager.TrackException("NoQuestionsFound", new Exception("No Questions found in LatestSurvey"));
            }

            return null;
        }

        private void AnswerYes()
        {
            UpdateQuestion(true);
        }

        private void AnswerNo()
        {
            UpdateQuestion(false);
        }

        private void StartStage()
        {
            if (_questionIndex > App.LatestSurvey?.Questions.Count)
            {
                EndSurvey();
            }

            //Move survey on by switching off the Stage View and reset increment
            ShowStage = false;

            UpdateCommands();
            _eventAggregator.GetEvent<QuestionChangedEvent>().Publish();
          
        }

        private void UpdateQuestion(bool value)
        {
            //Store answer in collection
            var properties = new Dictionary<string, string>
            {
                {"CurrentQuestionID", CurrentQuestion.ID.ToString()},
            };

            DependencyService.Get<IMetricsManagerService>().TrackEvent("SurveyNextQuestion", properties);

            // Save the response
            _response.QuestionResponses.Add(
                    new SurveyQuestionResponseDto
                    {
                        QuestionID = CurrentQuestion.ID,
                        StageID = CurrentQuestion.Stage_ID,
                        QuestionResponse = value,
                        QuestionStatement = CurrentQuestion.QuestionStatement
                    });

            //Select next question
            _questionIndex++;
            QuestionIncrement++;

            if (_questionIndex >= App.LatestSurvey?.Questions.Count)
            {
                EndSurvey();
            }
            else
            {
                //Select View based on stage
                var lastAnswerIndex = QuestionIndex(_response.QuestionResponses.OrderBy(q => q.QuestionID).Last().QuestionID);
                if (lastAnswerIndex != null && CurrentQuestion.Stage_ID > Question(lastAnswerIndex.Value).Stage_ID)
                {
                    //New stage so work out if we need to show the stage view
                    _stageIndex++;
                    //Reset stage question counter
                    QuestionIncrement = 1;

                    var stages = App.LatestSurvey?.Stages;
                    if (stages != null)
                    {
                        ShowStage = stages.Where(q => q.ID == CurrentQuestion.Stage_ID).First().ShowStageIntro;
                    }
                    else
                    {
                        //We should never get here otherwise we have no questions!
                        MetricsManager.TrackException("NoStagesFound", new Exception("No Stages found in LatestSurvey"));
                    }
                }
                else
                {
                    //Same stage so continue with questions
                    ShowStage = false;
                }

                UpdateCommands();
                _eventAggregator.GetEvent<QuestionChangedEvent>().Publish();
            }
        }

        public void UpdateCommands()
        {
            AnswerYesCommand.RaiseCanExecuteChanged();
            AnswerNoCommand.RaiseCanExecuteChanged();
        }

        public async Task UploadAndDeleteAsync()
        {
            IsBusy = true;

            try
            {
                await _response.UploadAsync();
                await _response.DeleteAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
