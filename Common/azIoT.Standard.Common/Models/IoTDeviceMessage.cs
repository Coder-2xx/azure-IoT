using System;
using System.Collections.Generic;
using System.Text;

namespace azIoT.Standard.Common.Models
{
    public class IoTDeviceMessage
    {
        public string Text;
        public IDictionary<string, string> Properties;
    }
}
