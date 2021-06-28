using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Environment.DTO
{
    public class DeviceRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ConnectionType { get; set; }
        public string RelayPhysicalAddress { get; set; }
        public string RelayNetworkAddress { get; set; }
    }
}
