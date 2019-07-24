using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControlLib.Dtos
{
    /// <summary>
    /// Model for the survey form to be displayed for each interviewee.
    /// </summary>
    public class SurveyDto
    {
        public int SurveyID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String IntroText { get; set; }
        public int Version { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public IList<SurveyQuestionDto> Questions { get; set; } = new List<SurveyQuestionDto>();
        public IList<SurveyStageDto> Stages { get; set; } = new List<SurveyStageDto>();
    }

    /// <summary>
    /// Client-side model for individual survey questions to be displayed for each interviewee.
    /// </summary>
    public class SurveyQuestionDto
    {
        public int QuestionID { get; set; }
        public int QuestionStageID { get; set; }
        public String QuestionNum { get; set; }
        public String QuestionText { get; set; }
        public String QuestionHelpText { get; set; }
        public String QuestionStatement { get; set; }
    }

    /// <summary>
    /// Client-side model for friendly names for each stage of the survey.
    /// </summary>
    public class SurveyStageDto
    {
        public int StageID { get; set; }
        public string StageText { get; set; }
        public bool ShowStageIntro { get; set; }
        public string StageTitle { get; set; }
    }
}
