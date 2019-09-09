using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControlLib.Dtos
{
    /// <summary>
    /// Client-side response model for the survey response from a given interviewee.
    /// Contains only identifiers needed for the responses.
    /// </summary>
    public class SurveyResponseDto
    {
        /// <summary>
        /// Database ID of Survey this response is for.
        /// Obtained from the server via the SurveyModel.
        /// </summary>
        public int SurveyID { get; set; }

        /// <summary>
        /// The version of the survey this response matches.
        /// </summary>
        public int Survey_Version { get; set; }

        /// <summary>
        /// Identifier of the Interviewer conducting this survey.
        /// </summary>
        public String UserID { get; set; }

        /// <summary>
        /// Identifier of the selected farm.
        /// </summary>
        public String FarmID { get; set; }

        /// <summary>
        /// Date the survey response was submitted.
        /// </summary>
        public DateTimeOffset SubmittedDate { get; set; }

        // While most questions only allow one answer, this will allow multiple-choice responses.
        /// <summary>
        /// Collection of answers.
        /// </summary>
        public IList<SurveyQuestionResponseDto> QuestionResponses { get; set; } = new List<SurveyQuestionResponseDto>();
        public Guid ResponseIdentifier { get; set; }
    }

    /// <summary>
    /// Client-side response model for individual survey question responses from a given interviewee.
    /// </summary>
    public class SurveyQuestionResponseDto
    {
        public int QuestionID { get; set; }
        public int StageID { get; set; }
        public bool QuestionResponse { get; set; }
        public string QuestionStatement { get; set; }
    }
}
