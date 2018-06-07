using System;
using System.Collections.Generic;
using System.Text;

namespace azIoT.Standard.Device.Common
{
    public static class Constants
    {
        public static class Messages
        {
            public const string Device_Client_Initialized = "Device client is initialized";
            public const string Device_Connected = "Device is connected";
        }

        public static class Workstation
        {
            public static class Commands
            {
                public const string LOCK = "LOCK";
                public const string LOGOFF = "LOGOFF";
                public const string CAPTURE_SCREEN = "CAPTURE_SCREEN";
            }
            public static class Messages
            {
                public const string LOCKED = "Workstation is locked by";
                public const string UNLOCKED = "Workstation is unlocked by";
                public const string LOGGEDIN = "Workstation is logged in by";
                public const string LOGGEDOFF = "Workstation is logged off by";
            }
        }
    }
}
