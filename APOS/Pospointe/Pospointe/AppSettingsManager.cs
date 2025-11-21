using Pospointe.LocalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe
{
    public static class AppSettingsManager
    {
        public static event Action OnSettingsUpdated;

        public static void ReloadPaymentSettings()
        {
            LoggedData.tiprequest = Properties.Settings.Default.ReqTip;
            LoggedData.comtype = Properties.Settings.Default.ComType;
            LoggedData.PaxIP = Properties.Settings.Default.PaxIP;
            LoggedData.PaxPort = Properties.Settings.Default.PaxPort;
            LoggedData.PaxComPort = Properties.Settings.Default.PaxComPort;
            LoggedData.PaxBaudRate = Properties.Settings.Default.PaxBaudRate;

            // Raise event to notify any listening windows
            OnSettingsUpdated?.Invoke();
        }

        public static bool CashDrawerEnabled
        {
            get => Properties.Settings.Default.CashDrawerEnabled;
            set
            {
                Properties.Settings.Default.CashDrawerEnabled = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
