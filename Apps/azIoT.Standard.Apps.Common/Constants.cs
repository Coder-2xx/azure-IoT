namespace azIoT.Standard.Apps.Common
{
    public static class Constants
    {
        public static class AppMessages
        {
            public const string APP_INITIALIZED = "App is initialized";
            public const string APP_LISTENING = "App started listening to device messages";
        }

        public static class ProcessorMessages
        {
            public const string PROCESSOR_INITIALIZED = "EventProcessor initialized.";

            public const string MESSAGE_RECEIVED = "Message received.   ";

            public const string ERROR = "Error on Partition";

            public const string PROCESSOR_SHUTTINGDOWN = "Processor Shutting Down.";

        }
    }
}
