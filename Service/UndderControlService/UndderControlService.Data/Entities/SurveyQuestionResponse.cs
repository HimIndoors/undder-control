using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndderControlService.Data.Entities
{
    public class SurveyQuestionResponse
    {
        public int ID { get; set; }

        [ForeignKey("SurveyResponse_ID")]
        public virtual SurveyResponse SurveyResponse { get; set; }
        [Required]
        public int SurveyResponse_ID { get; set; }

        [Required]
        public int QuestionID { get; set; }
        [Required]
        public int StageID { get; set; }

        public bool QuestionResponse { get; set; }
        public string QuestionStatement { get; set; }
    }
}
