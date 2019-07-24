using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControlLib.Dtos
{
    /// <summary>
    /// Basic farm details, only the ID, Name and HerdSize are required for the app to function.
    /// The rest are QoL fields for the vet/rep
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
        public Guid FarmIdentifier { get; set; }
    }
}
