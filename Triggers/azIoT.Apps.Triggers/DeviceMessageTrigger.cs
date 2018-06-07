
namespace azIoT.Apps.Triggers
{
    using Microsoft.AspNet.SignalR.Client;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.Azure.WebJobs.ServiceBus;
    using System;
    using System.IO;

    public static class DeviceMessageTrigger
    {
        [FunctionName("MessageFrom_Akshay__IoTDevice")]
        public static void Run([EventHubTrigger("iothub-akshay-us", Connection = "EventHub")] string myEventHubMessage, TraceWriter log)
        {
            log.Info($"MessageFrom_Akshay__IoTDevice: {myEventHubMessage}");

            try
            {
                var hubConnection = new HubConnection("http://localhost:57065/");
                IHubProxy hub = hubConnection.CreateHubProxy("DevicesHub");
                hubConnection.Start().Wait();
                hub.Invoke("SendMessage", myEventHubMessage).Wait();
            }
            catch (Exception exception)
            {

            }
        }


    }
}