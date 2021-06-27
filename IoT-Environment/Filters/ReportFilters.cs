using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Environment.Filters
{
    public class ReportFilters
    {
        public decimal LastMinutes { get; set; } = 5;
        public string DataType { get; set; }

        public override string ToString()
        {
            return $"{nameof(LastMinutes)}={LastMinutes},{nameof(DataType)}={DataType}";
        }
    }
}
