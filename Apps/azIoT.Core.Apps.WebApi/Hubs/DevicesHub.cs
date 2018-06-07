using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azIoT.Core.Apps.WebApi.Hubs
{
    public class DevicesHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("NewMessage", message);
        }
    }
}
