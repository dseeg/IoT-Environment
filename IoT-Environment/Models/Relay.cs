using System;
using System.Collections.Generic;

#nullable disable

namespace IoT_Environment.Models
{
    public partial class Relay
    {
        public Relay()
        {
            Devices = new HashSet<Device>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhysicalAddress { get; set; }
        public string NetworkAddress { get; set; }
        public DateTime DateRegistered { get; set; }
        public bool? Stale { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
    }
}
