using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe
{
    class CommSettingProperties
    {
        public string CommSetting { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        public string SerialPort { get; set; }

        public int BaudRate { get; set; }

        public int Timeout { get; set; }

        public CommSettingProperties()
        {

        }
    }
}
