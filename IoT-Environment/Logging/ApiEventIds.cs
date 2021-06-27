using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Environment.Logging
{
    public class ApiEventIds
    {
        public const int ReadAllReports = 1000;
        public const int ReadReport     = 1001;
        public const int UpdateReport   = 1002;
        public const int CreateReport   = 1003;
        public const int DeleteReport   = 1004;

        public const int ReadReportNotFound   = 2000;
        public const int UpdateReportNotFound = 2002;
        public const int DeleteReportNotFound = 2003;

        public const int UnknownException = 0;
    }
}
