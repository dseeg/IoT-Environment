using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Environment.DTO
{
    public class DeviceData
    {
        public string RelayPhysicalAddress { get; set; }
        public string RelayNetworkAddress { get; set; }
        public string DeviceAddress { get; set; }
        public int DataType { get; set; }
        public decimal Value { get; set; }
    }
}
