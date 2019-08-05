using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndderControlService.Data.Entities
{
    public class SurveyResponse
    {
        public int ID { get; set; }
        [ForeignKey("Survey_ID")]
        public virtual Survey Survey { get; set; }
        [Required]
        public int Survey_ID { get; set; }
        public int Survey_Version { get; set; }
        [ForeignKey("User_ID")]
        public virtual User User { get; set; }
        [Required]
        public int User_ID { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public Guid ResponseIdentifier { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public float Accuracy { get; set; }
        public virtual IList<SurveyQuestionResponse> QuestionResponses { get; set; } = new List<SurveyQuestionResponse>();
    }
}
