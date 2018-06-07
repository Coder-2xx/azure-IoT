namespace azIoT.Core.Controller.ConsoleApp
{
    using Microsoft.AspNetCore.SignalR.Client;
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
        static IConfiguration _config;

        static ServiceClient serviceClient;

        static async void ReceiveDataAsync()
        {
            try
            {
                ServiceProvider serviceProvider = ConfigureServices(new ServiceCollection());

                _logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
                _logger.LogTrace(common.Constants.Messages.Logger_Initialized);

                string iotHubEventHubConnectionString = _config.GetConnectionString("IOTHUB_EVENTHUB");
                string entityPath = _config["IOTHUB_ENDPOINT_ENTITYPATH"];

                string azureStorageConnectionString = _config.GetConnectionString("AZURE_STORAGE");
                string azureStorageContainerName = _config["AZURE_STORAGE_CONTAINERNAME"];

                serviceClient = ServiceClient.CreateFromConnectionString(_config.GetConnectionString("IOTHUB"));
                
                var notificationReceiver = serviceClient.GetFileNotificationReceiver();

                Console.WriteLine("\nReceiving file upload notification from service");
                while (true)
                {
                    var fileUploadNotification = await notificationReceiver.ReceiveAsync();
                    if (fileUploadNotification == null) continue;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Received file upload noticiation: {0}", string.Join(", ", fileUploadNotification.BlobName));
                    Console.ResetColor();

                    var connection = new HubConnectionBuilder()
           .WithUrl(_config.GetValue<string>("LIVE_FEED_HUB_URI"))
           .Build();

                    connection.StartAsync().Wait();
                    connection.InvokeAsync("SendMessage", $"File is uploaded at {fileUploadNotification.BlobUri}").Wait();

                    await notificationReceiver.CompleteAsync(fileUploadNotification);
                }

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
                _config = new ConfigurationBuilder()
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
                    string deviceId = _config["DEVICE_ID"];
                    CloudToDeviceMethod method = new CloudToDeviceMethod("lock");
                    method.ResponseTimeout = TimeSpan.FromSeconds(30);
                    CloudToDeviceMethodResult result = serviceClient.InvokeDeviceMethodAsync(deviceId, method).Result;

                    _logger.LogTrace("lock is invoked.");

                    Message message = new Message(Encoding.ASCII.GetBytes("System lock message"));
                    message.Properties.Add(common.Constants.Command, "LOCK");
                    message.Properties.Add(common.Constants.CommandType, "session");
                    message.Properties.Add(common.Constants.SessionIdentity, "all");

                    serviceClient.SendAsync(_config["DEVICE_ID"], message);
                }
            }
        }
    }
}