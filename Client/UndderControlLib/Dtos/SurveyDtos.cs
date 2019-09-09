using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControlLib.Dtos
{
    /// <summary>
    /// Client-side model for the survey form to be displayed for each interviewee.
    /// </summary>
    public class SurveyDto
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String IntroText { get; set; }
        public int Version { get; set; }
        public string Language { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public IList<SurveyQuestionDto> Questions { get; set; } = new List<SurveyQuestionDto>();
        public IList<SurveyStageDto> Stages { get; set; } = new List<SurveyStageDto>();
    }

    /// <summary>
    /// Client-side model for individual survey questions to be displayed for each interviewee.
    /// </summary>
    public class SurveyQuestionDto
    {
        public int ID { get; set; }
        public int Stage_ID { get; set; }
        public int QuestionNum { get; set; }
        public String QuestionText { get; set; }
        public String QuestionHelpText { get; set; }
        public String QuestionStatement { get; set; }
    }

    /// <summary>
    /// Client-side model for friendly names for each stage of the survey.
    /// </summary>
    public class SurveyStageDto
    {
        public int ID { get; set; }
        public string StageText { get; set; }
        public bool ShowStageIntro { get; set; }
        public string StageTitle { get; set; }
    }
}
