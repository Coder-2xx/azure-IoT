namespace azIoT.Apps.Triggers
{
    using azIoT.Standard.Apps.Common;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.Azure.WebJobs.ServiceBus;
    using System;

    public static class DeviceMessageTrigger
    {
        [FunctionName("MessageFrom_Akshay_IoTDevice")]
        public static void Run([EventHubTrigger("iothub-akshay-us", Connection = "EventHub")] string myEventHubMessage, TraceWriter log)
        {
            log.Info($"MessageFrom_Akshay_IoTDevice: {myEventHubMessage}");

            var connection = new HubConnectionBuilder()
           .WithUrl("http://localhost:57065/DevicesHub")
           .Build();

            connection.StartAsync().Wait();

            try
            {

                connection.InvokeAsync("SendMessage", myEventHubMessage);
            }
            catch (Exception exception)
            {

            }
        }
    }

}
