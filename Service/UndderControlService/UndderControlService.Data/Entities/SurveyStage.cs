using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndderControlService.Data.Entities
{
    public class SurveyStage
    {
        public int ID { get; set; }

        [ForeignKey("Survey_ID")]
        public virtual Survey Survey { get; set; }
        [Required]
        public int Survey_ID { get; set; }

        public string StageText { get; set; }
        public bool ShowStageIntro { get; set; }
        public string StageTitle { get; set; }
    }
}
