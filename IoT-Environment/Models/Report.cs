using System;
using System.Collections.Generic;

#nullable disable

namespace IoT_Environment.Models
{
    public partial class Report
    {
        public int Id { get; set; }
        public DateTime Posted { get; set; }
        public int Device { get; set; }
        public int DataType { get; set; }
        public decimal Value { get; set; }

        public virtual DataType DataTypeNavigation { get; set; }
        public virtual Device DeviceNavigation { get; set; }
    }
}
