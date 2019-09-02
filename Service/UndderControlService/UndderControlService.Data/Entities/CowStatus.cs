using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndderControlService.Data.Entities
{
    public class CowStatus
    {
        public int ID { get; set; }
        [ForeignKey("Farm_ID")]
        public virtual Farm Farm { get; set; }
        [Required]
        public int Farm_ID { get; set; }

        public bool InfectedAtDryOff { get; set; }
        public bool InfectedAtCalving { get; set; }
        public string CowIdentifier { get; set; }
        public DateTime? DateAddedDryOff { get; set; }
        public DateTime? DateAddedCalving { get; set; }
    }
}
