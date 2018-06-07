using azIoT.Standard.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace azIoT.Standard.Common.Contracts
{
    public interface IDeviceService
    {
        Task<List<IoTDevice>> GetDevicesAsync(string searchterm = null);

        Task SendMessageAsync(string message, string deviceId, IDictionary<string, string> properties);

        Task<int> CallMethodAsync(string action, string deviceId);
    }
}
