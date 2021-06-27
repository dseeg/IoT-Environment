using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Environment.Logging
{
    public class ApiEventIds
    {
        public const int UnknownException = 0;

        public const int ReadAllReports = 1000;
        public const int ReadReport     = 1001;
        public const int UpdateReport   = 1002;
        public const int CreateReport   = 1003;
        public const int DeleteReport   = 1004;

        public const int ReadAllRelays  = 1010;
        public const int ReadRelay      = 1011;
        public const int UpdateRelay    = 1012;
        public const int CreateRelay    = 1013;
        public const int DeleteRelay    = 1014;

        public const int ReadAllDevices = 1020;
        public const int ReadDevice     = 1021;
        public const int UpdateDevice   = 1022;
        public const int CreateDevice   = 1023;
        public const int DeleteDevice   = 1024;

        public const int ReadAllDataTypes   = 1030;
        public const int ReadDataType       = 1031;
        public const int UpdateDataType     = 1032;
        public const int CreateDataType     = 1033;
        public const int DeleteDataType     = 1034;

        public const int ReadReportNotFound     = 2000;
        public const int UpdateReportNotFound   = 2002;
        public const int DeleteReportNotFound   = 2003;

        public const int ReadRelayNotFound      = 2010;
        public const int UpdateRelayNotFound    = 2012;
        public const int DeleteRelayNotFound    = 2013;

        public const int ReadDeviceNotFound     = 2020;
        public const int UpdateDeviceNotFound   = 2022;
        public const int DeleteDeviceNotFound   = 2023;
    }
}
