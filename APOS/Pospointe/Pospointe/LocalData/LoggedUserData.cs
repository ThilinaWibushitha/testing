using Pospointe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe.LocalData
{
    class LoggedData
    {

        public static TblUser loggeduser = new();
        public static Register Reg = new();
        public static MyPOSUser myposuser = new();
        public static string StationID = "Default";
        public static string transbaseurl = clsConnections.transserverprimaryurl;
        public static bool Isoffline = false;
        public static TblBusinessInfo BusinessInfo = new TblBusinessInfo();


        public static string giftcardstoreid = "";


        public static string timcardstorestoreid = "f7e248ea-25f1-4f12-99f9-56d960196a98";

        //PAX CONNECTION

        public static bool tiprequest = Properties.Settings.Default.ReqTip;
        //public static string comtype = Properties.Settings.Default.ComType;
        //public static string PaxIP = Properties.Settings.Default.PaxIP;
        //public static string PaxPort = Properties.Settings.Default.PaxPort;
        public static string comtype = Properties.Settings.Default.ComType;
        public static string PaxIP = Properties.Settings.Default.PaxIP;
        public static string PaxPort = Properties.Settings.Default.PaxPort;
        public static string PaxComPort = Properties.Settings.Default.PaxIP;
        public static string PaxBaudRate = Properties.Settings.Default.PaxPort;

    }
}
