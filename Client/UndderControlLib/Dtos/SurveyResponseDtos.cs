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

        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// Unique GUID generated client-side for each response.
        /// Will ensure responses aren't uploaded multiple times due to bad connection, etc.
        /// </summary>
        public Guid ResponseIdentifier { get; set; }

        /// <summary>
        /// GPS coordinates where survey response is completed, if available.
        /// </summary>
        public GPSLocationDto GPSLocation { get; set; } = new GPSLocationDto();

        // While most questions only allow one answer, this will allow multiple-choice responses.
        public IList<SurveyQuestionResponseDto> QuestionResponses { get; set; } = new List<SurveyQuestionResponseDto>();
    }

    /// <summary>
    /// Client-side response model for individual survey question responses from a given interviewee.
    /// </summary>
    public class SurveyQuestionResponseDto
    {
        public SurveyQuestionResponseDto(int questionID, int stageID, bool questionResponse)
        {
            QuestionID = questionID;
            StageID = stageID;
            QuestionResponse = questionResponse;
        }

        public int QuestionID { get; set; }
        public int StageID { get; set; }
        public bool QuestionResponse { get; set; }
    }
}
