using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndderControlService.Data.Helper;

namespace UndderControlService.Data.Entities
{
    public class Survey
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IntroText { get; set; }
        public int Version { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Language { get; set; }
        public bool Active { get; set; }

        [CascadeDelete]
        public virtual IList<SurveyQuestion> Questions { get; set; }
        [CascadeDelete]
        public virtual IList<SurveyStage> Stages { get; set; }
    }
}
