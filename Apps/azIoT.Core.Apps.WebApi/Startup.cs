using azIoT.Standard.Apps.Common.Services;
using azIoT.Standard.Common.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using azIoT.Core.Apps.WebApi.Hubs;
using Microsoft.Azure.Devices;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using azIoT.Standard.Apps.Common;
using azIoT.Core.Apps.WebApi.Middlewares;

namespace azIoT.Core.Apps.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;

            //StartupListening();
        }

        public void StartupListening()
        {
            string iotHubEventHubConnectionString = Configuration.GetConnectionString("IOTHUB_EVENTHUB");
            string entityPath = Configuration["IOTHUB_ENDPOINT_ENTITYPATH"];

            string azureStorageConnectionString = Configuration.GetConnectionString("AZURE_STORAGE");
            string azureStorageContainerName = Configuration["AZURE_STORAGE_CONTAINERNAME"];

            EventHubsConnectionStringBuilder eventHubConnectionStringBuilder = new EventHubsConnectionStringBuilder(iotHubEventHubConnectionString)
            {
                EntityPath = entityPath
            };

            EventProcessorHost eventProcessorHost = new EventProcessorHost(entityPath, PartitionReceiver.DefaultConsumerGroupName, iotHubEventHubConnectionString, azureStorageConnectionString, azureStorageContainerName);

            eventProcessorHost.RegisterEventProcessorAsync<EventProcessor>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddCors();
            services.AddSignalR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseSignalR(routes =>
            {
                routes.MapHub<DevicesHub>("/DevicesHub");
            });

            if (Configuration.GetValue<bool>("ENABLE_AUTH"))
            {
                app.UseAzureADAuthMiddleware();
            }

            app.UseMvc();
        }
    }
}
