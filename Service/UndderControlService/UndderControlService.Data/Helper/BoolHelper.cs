using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndderControlService.Data.Helper
{
    public class BoolHelper
    {
        private readonly Random rnd;

        ///  <summary> 
        /// initializes a new instance of the <see cref = "BoolHelper" /> class. 
        ///  </ summary> 
        public BoolHelper()
        {
            rnd = new Random();
        }

        ///  <summary> 
        /// Gets the random boolean. 
        ///  </ summary> 
        ///  <returns> </ returns> 
        public bool GetRandomBoolean()
        {
            return rnd.Next(0, 2) == 0;
        }
    }
}
