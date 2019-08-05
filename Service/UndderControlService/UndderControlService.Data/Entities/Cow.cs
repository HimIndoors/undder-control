using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndderControlService.Data.Entities
{
    public class Cow
    {
        public int ID { get; set; }

        [ForeignKey("Farm_ID")]
        public virtual Farm Farm { get; set; }
        [Required]
        public int Farm_ID { get; set; }

        [ForeignKey("Process_ID")]
        public virtual CowProcess Process { get; set; }
        [Required]
        public int Process_ID { get; set; }

        [ForeignKey("Status_ID")]
        public virtual CowStatus Status { get; set; }
        [Required]
        public int Status_ID { get; set; }
        public string CowIdentifier { get; set; }
    }
}
