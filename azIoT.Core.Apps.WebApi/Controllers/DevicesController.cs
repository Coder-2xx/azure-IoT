using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using azIoT.Standard.Common.Contracts;
using azIoT.Standard.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Logging;

namespace azIoT.Core.Apps.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService, ILogger<DevicesController> logger)
        {
            this._deviceService = deviceService;
            this._logger = logger;
        }

        [HttpGet, Route("get/{searchterm?}")]
        [ProducesResponseType(200, Type = typeof(List<IoTDevice>))]
        public async Task<IActionResult> Get(string searchterm = null)
        {
            _logger.LogTrace($"API CALL GetDevices");

            List<IoTDevice> devices = await _deviceService.GetDevicesAsync(searchterm);

            return Ok(devices);
        }

        [HttpPost, Route("send/{message}/to/{deviceId?}")]
        public async Task<IActionResult> SendMessage([FromBody] IDictionary<string, string> properties, string message, string deviceId = null)
        {
            _logger.LogTrace($"API CALL SendMessage");

            try
            {
                await _deviceService.SendMessageAsync(message, deviceId, properties);
            }
            catch (Exception exception)
            {

            }
            return Ok();
        }

        [HttpPost, Route("call/{operation}/on/{deviceId}")]
        public async Task<IActionResult> CallOperation(string action, string deviceId)
        {
            _logger.LogTrace($"API CALL CallAction");

            await _deviceService.CallMethodAsync(action, deviceId);

            return Ok();
        }

        [HttpPost, Route("send/file")]
        public async Task<IActionResult> SendFile()
        {
            _deviceService.ToString();

            return Ok();
        }
    }
}