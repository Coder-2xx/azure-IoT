namespace azIoT.Apps.WebAPIs.Controllers
{
    using azIoT.Standard.Common.Contracts;
    using azIoT.Standard.Common.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
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

        [HttpPost, Route("sendto/{deviceId?}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SendMessage([FromBody] IoTDeviceMessage message, string deviceId = null)
        {
            _logger.LogTrace($"API CALL SendMessage");

            try
            {
                await _deviceService.SendMessageAsync(deviceId, message?.Text, message?.Properties);
            }
            catch (Exception exception)
            {
                return NotFound();
            }
            return Ok(true);
        }

        [HttpPost, Route("call/{operation}/on/{deviceId}")]
        public async Task<IActionResult> CallOperation(string operation, string deviceId)
        {
            _logger.LogTrace($"API CALL CallAction");

            await _deviceService.CallMethodAsync(operation, deviceId);

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