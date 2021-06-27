using System;
using System.Collections.Generic;

#nullable disable

namespace IoT_Environment.Models
{
    public partial class DataType
    {
        public DataType()
        {
            Reports = new HashSet<Report>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}
