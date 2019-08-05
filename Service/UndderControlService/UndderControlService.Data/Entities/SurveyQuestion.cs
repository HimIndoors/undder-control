using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndderControlService.Data.Entities
{
    public class SurveyQuestion
    {
        public int ID { get; set; }

        [ForeignKey("Survey_ID")]
        public virtual Survey Survey { get; set; }
        [Required]
        public int Survey_ID { get; set; }

        [ForeignKey("Stage_ID")]
        public virtual SurveyStage Stage { get; set; }
        [Required]
        public int Stage_ID { get; set; }
        public int QuestionNum { get; set; }
        public String QuestionText { get; set; }
        public String QuestionHelpText { get; set; }
        public String QuestionStatement { get; set; }
    }
}
