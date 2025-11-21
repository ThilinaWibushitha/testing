using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Printing;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Pospointe.LocalData;
using Pospointe.Models;

namespace Pospointe.posAssist
{
    public class PrinterQueueService
    {
        private bool _alertSent = false;

        // Email config
        private readonly string smtpServer = "smtp.office365.com";
        private readonly int smtpPort = 587;
        private readonly string senderEmail = "no_reply@asnitinc.com";
        private readonly string senderPassword = "Afshan@573184";
        private readonly string recipientList = "gokkul@asnitinc.com";



        private bool IsPrinterPortOpen(string ip, int port = 9100)
        {
            try
            {
                using (var client = new System.Net.Sockets.TcpClient())
                {
                    var result = client.BeginConnect(ip, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(1000));
                    if (!success)
                        return false;

                    client.EndConnect(result);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }



        private (string PortName, string Status) GetPrinterDetails(string printerName)
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

                foreach (ManagementObject printer in searcher.Get())
                {
                    string name = printer["Name"]?.ToString();
                    string port = printer["PortName"]?.ToString();
                    bool isOffline = Convert.ToBoolean(printer["WorkOffline"] ?? false);

                    if (!string.IsNullOrEmpty(name) && name.Equals(printerName, StringComparison.OrdinalIgnoreCase))
                    {
                        string status = "UNKNOWN";

                        if (port.StartsWith("IP_"))
                        {
                            // Ethernet printer → ping the IP
                            string ip = port.Replace("IP_", "");
                            bool reachable = IsPrinterPortOpen(ip);

                            status = reachable ? "ONLINE" : "OFFLINE ❌";
                        }
                        else if (port.StartsWith("USB") || port.StartsWith("Virtual"))
                        {
                            // USB printer → check WorkOffline
                            status = isOffline ? "OFFLINE ❌" : "ONLINE";
                        }
                        else
                        {
                            status = isOffline ? "OFFLINE ❌" : "ONLINE";
                        }

                        return (port, status);
                    }
                }
            }
            catch
            {
                // Ignore errors
            }

            return ("Unknown", "UNKNOWN");
        }


        public void Start()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        // Get printer names from settings
                        string receiptPrinter = Properties.Settings.Default.ReceiptPrinter;
                        string kitchenPrinter = Properties.Settings.Default.KitchenPrinter;

                        var monitoredPrinters = new Dictionary<string, int>();
                        LocalPrintServer printServer = new LocalPrintServer();
                        PrintQueueCollection printQueues = printServer.GetPrintQueues();

                        foreach (PrintQueue queue in printQueues)
                        {
                            queue.Refresh();

                            if (queue.Name == receiptPrinter || queue.Name == kitchenPrinter)
                            {
                                int jobs = queue.NumberOfJobs;
                                if (jobs > 0)
                                {
                                    monitoredPrinters[queue.Name] = jobs;
                                }
                            }
                        }

                        if (monitoredPrinters.Count > 0 && !_alertSent)
                        {
                            SendEmailAlert(monitoredPrinters);
                            _alertSent = true;

                            // Reset alert flag after 30 minutes
                            _ = Task.Delay(TimeSpan.FromMinutes(30)).ContinueWith(_ => _alertSent = false);
                        }
                    }
                    catch
                    {
                        // Optional: log error
                    }

                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            });
        }

        private string GetPrinterPort(string printerName)
        {
            try
            {
                var searcher = new System.Management.ManagementObjectSearcher(
                    "SELECT Name, PortName FROM Win32_Printer");

                foreach (var printer in searcher.Get())
                {
                    string name = printer["Name"]?.ToString();
                    string port = printer["PortName"]?.ToString();

                    if (!string.IsNullOrEmpty(name) && name.Equals(printerName, StringComparison.OrdinalIgnoreCase))
                    {
                        return port;
                    }
                }
            }
            catch
            {
                // ignore
            }

            return "Unknown";
        }


        private void SendEmailAlert(Dictionary<string, int> printerJobDetails)
        {
            try
            {
                string subject = "⚠️ POSPointe Printer Issue Detected";
                string body = $"Detected print jobs in monitored printers:\n\n";

                foreach (var printer in printerJobDetails)
                {
                    
                    var details = GetPrinterDetails(printer.Key);
                    
                    body += LoggedData.BusinessInfo.BusinessAddress;
                    body += $"🖨 {printer.Key}\n";
                    body += $" - Jobs in Queue: {printer.Value}\n";
                    body += $" - Connection: {details.PortName}\n";
                    body += $" - Status: {details.Status}\n\n";
                }



                MailMessage message = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                foreach (var recipient in recipientList.Split(','))
                {
                    message.To.Add(recipient.Trim());
                }

                SmtpClient client = new SmtpClient(smtpServer, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };

                client.Send(message);
            }
            catch
            {
                
            }
        }
    }
}
