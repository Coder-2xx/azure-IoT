namespace azIoT.Standard.Device.Common
{
    using Microsoft.Azure.Devices;
    using Microsoft.Azure.Devices.Client;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IWorkstation
    {
        void Init(DeviceClient deviceClient);

        void ExecuteSystemCommand(Message message);
    }
}