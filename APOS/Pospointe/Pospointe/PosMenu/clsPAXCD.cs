using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.SqlServer.Server;
using POSLink2;
using Pospointe.LocalData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pospointe.PosMenu
{
    class clsPAXCD
    {
        public static POSLink2.Terminal terminal;
        public static string ipaddr = LoggedData.PaxIP;
        public static string portnum = LoggedData.PaxPort;

        public static string bautrate { get; set; }

        public static string comtype = LoggedData.comtype;

        public static string comport { get; set; }
        public static async Task ShowItem(List<ItemGridModel> itms, string Tax, string GrandTotal)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                if (comtype == "PAX_IP")
            {
                CommSettingProperties commSettingProperties = new CommSettingProperties();
                commSettingProperties.Ip = ipaddr;
                commSettingProperties.Port = Int32.Parse(portnum);
                commSettingProperties.Timeout = Int32.Parse("-1");

                POSLink2.POSLink2 poslink = POSLink2.POSLink2.GetPOSLink2();
                POSLink2.CommSetting.TcpSetting commSetting = new POSLink2.CommSetting.TcpSetting();
                commSetting.Ip = commSettingProperties.Ip;
                commSetting.Port = commSettingProperties.Port;
                commSetting.Timeout = commSettingProperties.Timeout;
                terminal = poslink.GetTerminal(commSetting);

            }

            else
            {
                CommSettingProperties commSettingProperties = new CommSettingProperties();
                commSettingProperties.SerialPort = comport;
                commSettingProperties.BaudRate = Int32.Parse(bautrate);
                commSettingProperties.Timeout = Int32.Parse("-1");

                POSLink2.POSLink2 poslink = POSLink2.POSLink2.GetPOSLink2();
                SerialPortData serialPortData = new SerialPortData();
                serialPortData.SerialPortName = commSettingProperties.SerialPort;
                serialPortData.BaudRate = commSettingProperties.BaudRate;
                Uart uart = new Uart();
                uart.SetCommProperties(serialPortData);
                POSLink2.CommSetting.CustomerCommSetting commSetting = uart;
                commSetting.Timeout = commSettingProperties.Timeout;
                terminal = poslink.GetTerminal(commSetting);
            }
                   POSLink2.ExecutionResult executionResult = new POSLink2.ExecutionResult();
                //CLEAR EXISTING
                //CLEAR SCREEN FIRST
                POSLink2.Form.ClearMessageRsp clearMessageRsp = null;
                executionResult = terminal.Form.ClearMessage(out clearMessageRsp);
                if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.Ok)
                {
                    GetResponse(clearMessageRsp);
                }
                //SEND DATA
                // POSLink2.ExecutionResult executionResult = new POSLink2.ExecutionResult();
                POSLink2.Form.ShowItemReq showItemReq = SetShowItemReq(itms , Tax , GrandTotal);
                    POSLink2.Form.ShowItemRsp showItemRsp = null;
                    executionResult = terminal.Form.ShowItem(showItemReq, out showItemRsp);
                if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.Ok)
                {
                    GetResponse(showItemRsp);
                }

            };

            worker.RunWorkerAsync();
        }

        public static async Task ClearScreen()
        {
         
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                if (comtype == "PAX_IP")
                {
                    CommSettingProperties commSettingProperties = new CommSettingProperties();
                    commSettingProperties.Ip = ipaddr;
                    commSettingProperties.Port = Int32.Parse(portnum);
                    commSettingProperties.Timeout = Int32.Parse("-1");

                    POSLink2.POSLink2 poslink = POSLink2.POSLink2.GetPOSLink2();
                    POSLink2.CommSetting.TcpSetting commSetting = new POSLink2.CommSetting.TcpSetting();
                    commSetting.Ip = commSettingProperties.Ip;
                    commSetting.Port = commSettingProperties.Port;
                    commSetting.Timeout = commSettingProperties.Timeout;
                    terminal = poslink.GetTerminal(commSetting);

                }

                else
                {
                    CommSettingProperties commSettingProperties = new CommSettingProperties();
                    commSettingProperties.SerialPort = comport;
                    commSettingProperties.BaudRate = Int32.Parse(bautrate);
                    commSettingProperties.Timeout = Int32.Parse("-1");

                    POSLink2.POSLink2 poslink = POSLink2.POSLink2.GetPOSLink2();
                    SerialPortData serialPortData = new SerialPortData();
                    serialPortData.SerialPortName = commSettingProperties.SerialPort;
                    serialPortData.BaudRate = commSettingProperties.BaudRate;
                    Uart uart = new Uart();
                    uart.SetCommProperties(serialPortData);
                    POSLink2.CommSetting.CustomerCommSetting commSetting = uart;
                    commSetting.Timeout = commSettingProperties.Timeout;
                    terminal = poslink.GetTerminal(commSetting);
                }
                POSLink2.ExecutionResult executionResult = new POSLink2.ExecutionResult();
                //CLEAR SCREEN FIRST
                POSLink2.Manage.ResetRsp clearMessageRsp = null;
                executionResult = terminal.Manage.Reset(out clearMessageRsp);
                if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.Ok)
                {
                    GetResponse(clearMessageRsp);
                }


            };

            worker.RunWorkerAsync();
        }

        public static string TrimMiddle(string input, int maxLength = 20)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
                return input;

            // Length of the ellipsis
            const string ellipsis = "...";
            int charsToShow = maxLength - ellipsis.Length;
            int frontChars = charsToShow / 2;
            int backChars = charsToShow - frontChars;

            return input.Substring(0, frontChars) + ellipsis + input.Substring(input.Length - backChars);
        }

        public static string changeamountostring(decimal price)
        {
            // Multiply by 100 to get cents, then format with leading zeros for 8 total digits


            // Use Math.Abs to convert negative values to positive, then format to 8 digits
            //  return ((int)(Math.Abs(price) * 100)).ToString("D8");

            if (price > 0)
            {
                return ((int)(price * 100)).ToString("D8");
            }

            else
            {
                return "-" +((int)(Math.Abs(price) * 100)).ToString("D8");
            }

        }

        public static string changeamountostringUnitPRice(decimal price)
        {
            // Multiply by 100 to get cents, then format with leading zeros for 8 total digits


            // Use Math.Abs to convert negative values to positive, then format to 8 digits
            //  return ((int)(Math.Abs(price) * 100)).ToString("D8");

            if (price > 0)
            {
                return ((int)(price * 100)).ToString("D8");
            }

            else
            {
                return  ((int)(Math.Abs(price) * 100)).ToString("D8");
            }

        }
        private static POSLink2.Form.ShowItemReq SetShowItemReq(List<ItemGridModel> itms , string Tax , string GrandTotal)
        {
            
          //  decimal TAXindec = Convert.ToDecimal(Tax);
           // decimal GrTotal = Convert.ToDecimal(GrandTotal);
            POSLink2.Form.ShowItemReq request = new POSLink2.Form.ShowItemReq();
            request.Title = "Items";
            request.Topdown = "Y";
            request.TaxLine = Tax;
            request.TotalLine = GrandTotal;

            POSLink2.Util.ItemDetail[] itemsToShow = itms.Select(itm => new POSLink2.Util.ItemDetail
            {
                ProductName = TrimMiddle( itm.ItemName),
                //PluCode = itm.Itemid,
                //Price =  (itm.TotalPrice * 100).ToString(),
                //Quantity = (Convert.ToInt32(itm.Quantity)).ToString(),
                //Tax = "",
                //Unit = "0",
                //UnitPrice = ( itm.Price * 100).ToString(),
                //ProductImageName = "",
                //ProductImageDescription = ""
                PluCode = itm.Itemid,
                Price = changeamountostring(itm.TotalPrice),
                Quantity = itm.Quantity.ToString(),
                Tax = "",
                Unit = "0",
                UnitPrice = changeamountostringUnitPRice(itm.Price),
                ProductImageName = "",
                ProductImageDescription = ""
            }).ToArray();

            request.SetItemDetail(itemsToShow);
           // request.ItemDetail = "1,1,1,1,1,1,1";
             
           // }
            //else
            //{
            //    request.SetItemDetail(_formData.ItemDetailData);
            //}
            request.LineItemAction = "";
            request.ItemIndex = "";
            return request;
        }

        public static void GetResponse(POSLink2.Form.ShowItemRsp response)
        {
            if (response.ResponseCode != "000000")
            {
                MessageBox.Show($"{response.ResponseCode} : {response.ResponseMessage}");
            }

        }


        public static void GetResponse(POSLink2.Form.ClearMessageRsp response)
        {
            //FormData formData = FormData.GetFormData();
            string responsecode = response.ResponseCode;
            string responsemsg = response.ResponseMessage;

           // MessageBox.Show(responsecode);
        }

        public static void GetResponse(POSLink2.Manage.ResetRsp response)
        {
            //ManageData manageData = ManageData.GetManageData();
            ////Normal
            //manageData.ResetRspNormalData[0] = response.ResponseCode;
            //manageData.ResetRspNormalData[1] = response.ResponseMessage;

            if (response.ResponseCode != "000000")
            {
                MessageBox.Show($"{response.ResponseCode} : {response.ResponseMessage}");
            }
        }

    }
}
