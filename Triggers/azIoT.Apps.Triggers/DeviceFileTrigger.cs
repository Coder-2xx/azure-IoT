using Microsoft.AspNet.SignalR.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace azIoT.Apps.Triggers
{
    public class DeviceFileTrigger
    {
        [FunctionName("FileFrom_Akshay__IoTDevice")]
        public static void Run([BlobTrigger("files/{name}", Connection = "FileStorage")] Stream myBlob, string name, TraceWriter log)
        {
            log.Info($"MessageFrom_Akshay__IoTDevice: {name}");

            try
            {
                HubConnection connection = new HubConnection("http://localhost:57065/deviceshub/SendMessage");
                connection.Start().Wait();
                connection.Send($"File is uploaded at <a href=https://akshayiotusfilestorage.blob.core.windows.net/files/{name} />");
            }
            catch (Exception exception)
            {

            }
        }
    }
}
