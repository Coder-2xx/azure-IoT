namespace azIoT.Standard.Apps.Common
{
    using System;
    using Microsoft.Azure.EventHubs.Processor;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.Azure.EventHubs;
    using System.Text;
    using Microsoft.AspNetCore.SignalR.Client;

    public class EventProcessor : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"{Constants.ProcessorMessages.PROCESSOR_SHUTTINGDOWN} Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"{Constants.ProcessorMessages.PROCESSOR_INITIALIZED} Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"{Constants.ProcessorMessages.ERROR}: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            var connection = new HubConnectionBuilder()
           .WithUrl("https://aziotdevicemanagementwebapi.azurewebsites.net/DevicesHub")
           .Build();

            connection.StartAsync().Wait();

            foreach (var eventData in messages)
            {
                try
                {
                    var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                    Console.WriteLine($"{Constants.ProcessorMessages.MESSAGE_RECEIVED}Partition: '{context.PartitionId}', Data: '{data}'");

                    connection.InvokeAsync("SendMessage", data);
                }
                catch (Exception exception)
                {

                }
            }

            return context.CheckpointAsync();
        }
    }
}
