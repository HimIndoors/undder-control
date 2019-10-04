using FFImageLoading.Forms;
using Prism.Events;
using Prism.Navigation;
using System;
using UndderControl.Events;
using UndderControl.Services;
using UndderControl.Text;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class SurveyPage : ContentPage
    {
        private readonly SurveyPageViewModel _viewModel;

        public SurveyPage(IEventAggregator ea)
        {
            InitializeComponent();
            StartQuestionsButton.Text = AppTextResource.SurveyStartQuestionsButton;
            YesButton.Text = AppTextResource.SurveyYesButton.ToUpper(); //Force Uppercase
            NoButton.Text = AppTextResource.SurveyNoButton.ToUpper(); //Force Uppercase
            _viewModel = BindingContext as SurveyPageViewModel;
            UpdateQuestion();
            ea.GetEvent<QuestionChangedEvent>().Subscribe(UpdateQuestion);
        }

        private async void UpdateQuestion()
        {
            try
            {
                var stage = _viewModel.CurrentStage;
                Title = stage.StageTitle.ToUpper(); //Force uppercase

                if (_viewModel.ShowStage)
                {
                    //Show Stage view
                    SurveyStageView.IsVisible = true;
                    SurveyQuestionView.IsVisible = false;
                    StageBackground.IsVisible = true;

                    StageTitle.Text = stage.StageTitle.ToUpper(); //Force uppercase
                    StageText.Text = stage.StageText.ToUpper(); //Force uppercase
                    if (stage.ID == 2)
                    {
                        Scoring.IsVisible = true;
                    }
                    else
                    {
                        Scoring.IsVisible = false;
                    }
                }
                else
                {
                    //Show Question view
                    SurveyStageView.IsVisible = false;
                    SurveyQuestionView.IsVisible = true;
                    StageBackground.IsVisible = false;

                    // Render the current question
                    var q = _viewModel.CurrentQuestion;

                    // Update question text
                    //Set fontsize!
                    int textLength = q.QuestionText.Length;
                    if (textLength <= 79)
                    {
                        _viewModel.FontSize = 34;
                    }
                    else if (textLength > 79 && textLength <= 90)
                    {
                        _viewModel.FontSize = 30;
                    }
                    else if (textLength > 90 && textLength <= 100)
                    {
                        _viewModel.FontSize = 30;
                    }
                    else if (textLength > 100 && textLength <= 120)
                    {
                        _viewModel.FontSize = 28;
                    }
                    else if (textLength > 120 && textLength <= 150)
                    {
                        _viewModel.FontSize = 24;
                    }
                    else
                    {
                        _viewModel.FontSize = 22;
                    }
                    QuestionLabel.Text = q.QuestionText.ToUpper(); //Force uppercase

                    HelpTextLabel.Text = q.QuestionHelpText;

                    RenderProgress();
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMetricsManagerService>().TrackException("SurveyQuestionFailed", ex);
                await DisplayAlert("Error", "Something went wrong when loading this question.", "OK");
            }
        }

        private void RenderProgress()
        {
            var currentPos = _viewModel.QuestionIncrement;
            var totalPos = _viewModel.StageQuestionCount;
            var lightRes = "resource://UndderControl.Svg.progresslight.svg";
            var darkRes = "resource://UndderControl.Svg.progressdark.svg";

            if (totalPos == 2)
            {
                ProgressPip1.IsVisible = true;
                ProgressPip2.IsVisible = true;
                ProgressPip3.IsVisible = false;
                ProgressPip4.IsVisible = false;
                ProgressPip5.IsVisible = false;
                switch (currentPos)
                {
                    case 1:
                        ProgressPip1.Source = darkRes;
                        ProgressPip2.Source = lightRes;
                        break;

                    case 2:
                        ProgressPip1.Source = darkRes;
                        ProgressPip2.Source = darkRes;
                        break;
                }
            }
            else
            {
                ProgressPip1.IsVisible = true;
                ProgressPip2.IsVisible = true;
                ProgressPip3.IsVisible = true;
                ProgressPip4.IsVisible = true;
                ProgressPip5.IsVisible = true;

                switch (currentPos)
                {
                    case 1:
                        ProgressPip1.Source = darkRes;
                        ProgressPip2.Source = lightRes;
                        ProgressPip3.Source = lightRes;
                        ProgressPip4.Source = lightRes;
                        ProgressPip5.Source = lightRes;
                        break;

                    case 2:
                        ProgressPip1.Source = darkRes;
                        ProgressPip2.Source = darkRes;
                        ProgressPip3.Source = lightRes;
                        ProgressPip4.Source = lightRes;
                        ProgressPip5.Source = lightRes;
                        break;

                    case 3:
                        ProgressPip1.Source = darkRes;
                        ProgressPip2.Source = darkRes;
                        ProgressPip3.Source = darkRes;
                        ProgressPip4.Source = lightRes;
                        ProgressPip5.Source = lightRes;
                        break;

                    case 4:
                        ProgressPip1.Source = darkRes;
                        ProgressPip2.Source = darkRes;
                        ProgressPip3.Source = darkRes;
                        ProgressPip4.Source = darkRes;
                        ProgressPip5.Source = lightRes;
                        break;

                    case 5:
                        ProgressPip1.Source = darkRes;
                        ProgressPip2.Source = darkRes;
                        ProgressPip3.Source = darkRes;
                        ProgressPip4.Source = darkRes;
                        ProgressPip5.Source = darkRes;
                        break;
                }
            }
        }
    }
}