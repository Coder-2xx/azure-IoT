namespace azIoT.Standard.Device.Common
{
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Extensions.Logging;
    using Microsoft.Win32;
    using System;
    using System.Drawing;
    using System.IO;
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
                    message = new Message(Encoding.ASCII.GetBytes($"{Constants.Workstation.Messages.LOCKED} {this._deviceOwner}"));
                }
                else if (e.Reason.Equals(SessionSwitchReason.SessionUnlock))
                {
                    message = new Message(Encoding.ASCII.GetBytes($"{Constants.Workstation.Messages.UNLOCKED} {this._deviceOwner}"));
                }
                else if (e.Reason.Equals(SessionSwitchReason.SessionLogon))
                {
                    message = new Message(Encoding.ASCII.GetBytes($"{Constants.Workstation.Messages.LOGGEDIN} {this._deviceOwner}"));
                }
                else if (e.Reason.Equals(SessionSwitchReason.SessionLogoff))
                {
                    message = new Message(Encoding.ASCII.GetBytes($"{Constants.Workstation.Messages.LOGGEDOFF} {this._deviceOwner}"));
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

        private void CaptureScreen()
        {
            Bitmap memoryImage;
            memoryImage = new Bitmap(1000, 900);
            Size s = new Size(memoryImage.Width, memoryImage.Height);

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);

            memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);

            string fileName = $"Screenshot_{DateTime.Now.ToString("(dd_MMMM_hh_mm_ss_tt)")}.png";

            memoryImage.Save(fileName);

            using (var sourceData = new FileStream($"{fileName}", FileMode.Open))
            {
                _deviceClient.UploadToBlobAsync(fileName, sourceData).Wait();
            }
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

        public void ExecuteSystemCommand(string command)
        {
            switch (command.ToUpper())
            {
                case Constants.Workstation.Commands.LOGOFF:
                    Logoff();
                    break;

                case Constants.Workstation.Commands.LOCK:
                    Lock();
                    break;

                case Constants.Workstation.Commands.CAPTURE_SCREEN:
                    CaptureScreen();
                    break;

                default:
                    break;
            }
        }
    }
}
