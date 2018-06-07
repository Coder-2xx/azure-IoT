//namespace azIoT.Apps.Triggers
//{
//    using azIoT.Standard.Apps.Common;
//    using Microsoft.AspNetCore.SignalR.Client;
//    using Microsoft.Azure.WebJobs;
//    using Microsoft.Azure.WebJobs.Host;
//    using Microsoft.Azure.WebJobs.ServiceBus;
//    using System;
//    using System.IO;

//    public static class DeviceFileTrigger
//    {
//        [FunctionName("FileFrom_Akshay_IoTDevice")]
//        public static void Run([BlobTrigger("https://akshayiotusfilestorage.blob.core.windows.net/files")] Stream myBlob, string name, TraceWriter log)
//        {
//            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
//        }
//    }

//}
