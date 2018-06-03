using System;
using System.Collections.Generic;
using System.Text;

namespace azIoT.Standard.Device.Common
{
    public static class Constants
    {
        public static class Messages
        {
            public static readonly string Device_Client_Initialized = "Device client is initialized";
            public static readonly string Device_Connected = "Device is connected";
        }

        public static class WorkstationCommands
        {
            public const string LOCK = "LOCK";
            public const string LOGOFF = "LOGOFF";
            public const string CAPTURE_SCREEN = "CAPTURE_SCREEN";
        }
    }
}
