namespace azIoT.Core.Controller.ConsoleApp
{
    using Microsoft.Azure.Devices;
    using Microsoft.Azure.EventHubs;
    using Microsoft.Azure.EventHubs.Processor;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;
    using System;
    using System.Text;
    using common = azIoT.Standard.Common;
    using controller_common = azIoT.Standard.Apps.Common;

    class Program
    {
        static ILogger _logger;
        static IConfiguration _appConfig;

        static ServiceClient serviceClient;

        static async void ReceiveDataAsync()
        {
            try
            {
                ServiceProvider serviceProvider = ConfigureServices(new ServiceCollection());

                _logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
                _logger.LogTrace(common.Constants.Messages.Logger_Initialized);

                string iotHubEventHubConnectionString = _appConfig.GetConnectionString("IOTHUB_EVENTHUB");
                string entityPath = _appConfig["IOTHUB_ENDPOINT_ENTITYPATH"];

                string azureStorageConnectionString = _appConfig.GetConnectionString("AZURE_STORAGE");
                string azureStorageContainerName = _appConfig["AZURE_STORAGE_CONTAINERNAME"];


                RegistryManager registryManager = RegistryManager.CreateFromConnectionString("HostName=IoTHub-Akshay-US.azure-devices.net;SharedAccessKeyName=registryRead;SharedAccessKey=3hrCj6LtcuIsB9y0EVwcDnRAo6ePaCigWtTivBQYgBI=");

                var devices = await registryManager.GetDevicesAsync(100); // Time 1 sek

                foreach (var item in devices)
                {
                    Console.WriteLine("Divice id: " + item.Id + ", Connection state: " + item.ConnectionState);
                }
                //Console.WriteLine("\nNumber of devices: " + devices.());

                serviceClient = ServiceClient.CreateFromConnectionString(_appConfig.GetConnectionString("IOTHUB"));

                EventHubsConnectionStringBuilder eventHubConnectionStringBuilder = new EventHubsConnectionStringBuilder(iotHubEventHubConnectionString)
                {
                    EntityPath = entityPath
                };

                EventProcessorHost eventProcessorHost = new EventProcessorHost(entityPath, PartitionReceiver.DefaultConsumerGroupName, iotHubEventHubConnectionString, azureStorageConnectionString, azureStorageContainerName);

                await eventProcessorHost.RegisterEventProcessorAsync<controller_common.EventProcessor>();

                _logger.LogTrace(common.Constants.Messages.Logger_Initialized);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        static ServiceProvider ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog();
            NLog.LogManager.LoadConfiguration("nlog.config");

            return serviceProvider;
        }

        static void Main(string[] args)
        {
            try
            {
                _appConfig = new ConfigurationBuilder()
                    .AddJsonFile("appconfig.json", true, true)
                    .Build();

                ReceiveDataAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                Console.WriteLine("Press ENTER to send LOCK message...");
                while (true)
                {
                    Console.ReadLine();
                    string deviceId = _appConfig["DEVICE_ID"];
                    CloudToDeviceMethod method = new CloudToDeviceMethod("lock");
                    method.ResponseTimeout = TimeSpan.FromSeconds(30);
                    CloudToDeviceMethodResult result = serviceClient.InvokeDeviceMethodAsync(deviceId, method).Result;

                    _logger.LogTrace("lock is invoked.");

                    Message message = new Message(Encoding.ASCII.GetBytes("System lock message"));
                    message.Properties.Add(common.Constants.Command, "LOCK");
                    message.Properties.Add(common.Constants.CommandType, "session");
                    message.Properties.Add(common.Constants.SessionIdentity, "all");

                    serviceClient.SendAsync(_appConfig["DEVICE_ID"], message);
                }
            }
        }
    }
}