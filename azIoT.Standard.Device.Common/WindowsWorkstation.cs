namespace azIoT.Standard.Device.Common
{
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Extensions.Logging;
    using Microsoft.Win32;
    using System;
    using System.Security.Principal;
    using System.Text;

    public class WindowsWorkstation : IWorkstation
    {
        private DeviceClient _deviceClient;
        private readonly string _deviceOwner;
        private readonly ILogger<WindowsWorkstation> _logger;

        private async void OnSessionSwitch(Object sender, SessionSwitchEventArgs e)
        {
            try
            {
                _logger.LogTrace($"{Convert.ToString(e.Reason)}");

                Message message = null;

                if (e.Reason.Equals(SessionSwitchReason.SessionLock))
                {
                    message = new Message(Encoding.ASCII.GetBytes($"Session|Lock|{this._deviceOwner}"));
                }
                else if (e.Reason.Equals(SessionSwitchReason.SessionUnlock))
                {
                    message = new Message(Encoding.ASCII.GetBytes($"Session|Unlock|{this._deviceOwner}"));
                }
                else if (e.Reason.Equals(SessionSwitchReason.SessionLogon))
                {
                    message = new Message(Encoding.ASCII.GetBytes($"Session|Logon|{this._deviceOwner}"));
                }
                else if (e.Reason.Equals(SessionSwitchReason.SessionLogoff))
                {
                    message = new Message(Encoding.ASCII.GetBytes($"Session|Logoff|{this._deviceOwner}"));
                }
                await _deviceClient.SendEventAsync(message);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
            }
            finally
            {

            }
        }

        private void Lock()
        {
            System.Diagnostics.Process.Start("Rundll32.exe", "User32.dll,LockWorkStation");
        }

        private void Logoff()
        {
            System.Diagnostics.Process.Start("Rundll32.exe", "User32.dll,ExitWindowsEx,0,0");
        }

        public WindowsWorkstation(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<WindowsWorkstation>();
            this._deviceOwner = WindowsIdentity.GetCurrent().Name;
        }

        public void Init(DeviceClient deviceClient)
        {
            this._deviceClient = deviceClient;
            SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(OnSessionSwitch);
        }

        public void ExecuteSystemCommand(Message message)
        {
            string commandType = null != message.Properties && message.Properties.ContainsKey("command_type") ? Convert.ToString(message.Properties["command_type"]) : string.Empty;

            if (commandType.Equals("session", StringComparison.InvariantCultureIgnoreCase))
            {
                string sessionIdentity = message.Properties.ContainsKey("session_identity") ? Convert.ToString(message.Properties["session_identity"]) : string.Empty;

                if (sessionIdentity.Equals("all", StringComparison.InvariantCultureIgnoreCase) || sessionIdentity.Equals(this._deviceOwner, StringComparison.InvariantCultureIgnoreCase))
                {
                    switch (message.Properties["command"].ToUpper())
                    {
                        case "LOGOFF":
                            Logoff();
                            break;

                        case "LOCK":
                            Lock();
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}
