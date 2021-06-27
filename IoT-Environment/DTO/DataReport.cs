using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Environment.DTO
{
    public class DataReport
    {
        public DateTime PostedOn { get; set; }
        public decimal Value { get; set; }
        public string DataType { get; set; }
        public string DataUnits { get; set; }
        public string RelayName { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
    }
}
