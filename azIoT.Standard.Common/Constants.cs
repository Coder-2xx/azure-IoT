using System;

namespace azIoT.Standard.Common
{
    public static class Constants
    {
        public static readonly string Command = "COMMAND";
        public static readonly string CommandType = "COMMAND_TYPE";
        public static readonly string SessionIdentity = "session_identity";

        public static class CommandTypes
        {
            public static readonly string Session = "SESSION";
        }

        public static class Messages
        {
            public static readonly string Logger_Initialized = "Logger is initialized.";
        }

    }
}
