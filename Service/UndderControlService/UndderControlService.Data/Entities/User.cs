using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndderControlService.Data.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
    }
}
