using IoT_Environment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IoT_Environment.Extensions
{
    public static class RelayExtension
    { 
        public static bool TryUpdateNetworkAddress(this Relay relay, string networkAddr)
        {
            if (relay.NetworkAddress != networkAddr)
            {
                relay.NetworkAddress = networkAddr;
                return true;
            }

            return false;
        }
    }
}
