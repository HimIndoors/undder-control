using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControlLib.Dtos
{
    public class CowStatusDto
    {
        public int ID { get; set; }
        public string CowId { get; set; }
        public bool Infected { get; set; }
        public int Farm_ID { get; set; }
    }
}
