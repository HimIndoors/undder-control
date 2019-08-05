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
        [Key]
        public int ID { get; set; }

        [ForeignKey("SurveyResponse_ID")]
        public virtual SurveyResponse SurveyResponse { get; set; }
        [Required]
        public int SurveyResponse_ID { get; set; }

        [Required]
        public int Question_ID { get; set; }

        [Required]
        public int Stage_ID { get; set; }

        public bool QuestionResponse { get; set; }
    }
}
