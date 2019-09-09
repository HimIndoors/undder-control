using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControlLib.Dtos
{
    /// <summary>
    /// Client-side model farm details
    /// Only the ID and Name are required for the app to function, the rest are QoL fields for the vet/rep
    /// </summary>
    public class FarmDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public int HerdSize { get; set; }
        public string Type { get; set; }
    }
}
