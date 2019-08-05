using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndderControlService.Data.Entities
{
    public class Farm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public int HerdSize { get; set; }
        public string Type { get; set; }

        [ForeignKey("User_ID")]
        public virtual User User { get; set; }
        [Required]
        public int User_ID { get; set; }
    }
}
