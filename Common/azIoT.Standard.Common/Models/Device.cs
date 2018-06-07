using System;
using System.Collections.Generic;
using System.Text;

namespace azIoT.Standard.Common.Models
{
    public class IoTDevice
    {
        public IoTDevice(string Id, string ConnectionState)
        {
            this.Id = Id;
            this.ConnectionState = ConnectionState;
        }

        public string Id { get; set; }
        public string ConnectionState { get; set; }
    }
}