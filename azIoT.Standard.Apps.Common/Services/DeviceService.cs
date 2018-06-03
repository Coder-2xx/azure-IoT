using azIoT.Standard.Common.Contracts;
using azIoT.Standard.Common.Models;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace azIoT.Standard.Apps.Common.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly ILogger<DeviceService> _logger;
        private readonly IConfiguration _configuration;

        public DeviceService(ILogger<DeviceService> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }

        public async Task<List<IoTDevice>> GetDevicesAsync(string searchterm = null)
        {
            List<IoTDevice> devices = null;
            RegistryManager registryManager = null;

            try
            {
                devices = new List<IoTDevice>();

                registryManager = RegistryManager.CreateFromConnectionString("HostName=IoTHub-Akshay-US.azure-devices.net;SharedAccessKeyName=registryRead;SharedAccessKey=3hrCj6LtcuIsB9y0EVwcDnRAo6ePaCigWtTivBQYgBI=");

                IEnumerable<Microsoft.Azure.Devices.Device> iotDevices = await registryManager.GetDevicesAsync(100);
                foreach (Microsoft.Azure.Devices.Device iotDevice in iotDevices)
                {
                    devices.Add(new IoTDevice(iotDevice.Id, Convert.ToString(iotDevice.ConnectionState)));
                }
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
            }
            finally
            {
                registryManager?.Dispose();
            }
            return devices;
        }

        public async Task SendMessageAsync(string deviceId, string message, IDictionary<string, string> properties = null)
        {
            Message c2dMessage = new Message(Encoding.ASCII.GetBytes(message));

            if (null != properties)
            {
                foreach (var property in properties)
                {
                    c2dMessage.Properties.Add(property.Key, property.Value);
                }
            }

            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(_configuration.GetConnectionString("IOTHUB"));

            await serviceClient.SendAsync(deviceId, c2dMessage);
        }

        public async Task<int> CallMethodAsync(string action, string deviceId)
        {
            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(_configuration.GetConnectionString("IOTHUB"));
            CloudToDeviceMethodResult c2dMethodRes = await serviceClient.InvokeDeviceMethodAsync(deviceId, new CloudToDeviceMethod(action));

            return c2dMethodRes.Status;
        }
    }
}
