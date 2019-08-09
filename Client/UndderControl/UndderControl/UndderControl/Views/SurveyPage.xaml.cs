﻿using Prism.Events;
using Prism.Navigation;
using System;
using UndderControl.Events;
using UndderControl.Services;
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
            _viewModel = BindingContext as SurveyPageViewModel;
            UpdateQuestion();
            ea.GetEvent<QuestionChangedEvent>().Subscribe(UpdateQuestion);
        }

        private async void UpdateQuestion()
        {
            try
            {
                var stage = _viewModel.CurrentStage;
                Title = stage.StageTitle;

                if (_viewModel.ShowStage)
                {
                    //Show Stage view
                    SurveyStageView.IsVisible = true;
                    SurveyQuestionView.IsVisible = false;

                    StageTitle.Text = stage.StageTitle;
                    StageTitle.Text = stage.StageText;
                }
                else
                {
                    //Show Question view
                    SurveyStageView.IsVisible = false;
                    SurveyQuestionView.IsVisible = true;

                    // Render the current question
                    var q = _viewModel.CurrentQuestion;

                    // Update question text
                    QuestionLabel.Text = q.QuestionText;
                    HelpTextLabel.Text = q.QuestionHelpText;

                    Increment.Text = _viewModel.QuestionIncrement.ToString();
                    StageQuestionCount.Text = _viewModel.StageQuestionCount.ToString();
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMetricsManagerService>().TrackException("SurveyQuestionFailed", ex);
                await DisplayAlert("Error", "Something went wrong when loading this question.", "OK");
            }
        }
    }
}
