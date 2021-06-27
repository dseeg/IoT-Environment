using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Environment.DTO
{
    public class RelayRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhysicalAddress { get; set; }
        public string NetworkAddress { get; set; }
    }
}
