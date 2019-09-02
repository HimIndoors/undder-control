using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControlLib.Dtos
{
    public class CowStatusDto
    {
        public int ID { get; set; }
        public int Farm_ID { get; set; }
        public bool InfectedAtDryOff { get; set; }
        public bool InfectedAtCalving { get; set; }
        public string CowIdentifier { get; set; }
        public DateTime? DateAddedDryOff { get; set; }
        public DateTime? DateAddedCalving { get; set; }
    }
}
