using Microsoft.SqlServer.TransactSql.ScriptDom;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pospointe.Services
{
    public class UpdateService
    {
        public static bool CheckForUpdate()
        {
            var options = new RestClientOptions("https://asnitagentapi.azurewebsites.net")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Updater/getnewversion", Method.Get);
            RestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                TblVersion myDeserializedClass = JsonConvert.DeserializeObject<TblVersion>(response.Content);
                Console.WriteLine("Latest Version is : " + myDeserializedClass.CurrentVersion);
               
                decimal latestversion = Convert.ToDecimal( myDeserializedClass.CurrentVersion);
                decimal thisversion = Convert.ToDecimal(clsConnections.thisversion);
                if (latestversion > thisversion)
                {
                    return true;
                }
                else { 
                
                return false;
                }

            }
            else
            {
                MessageBox.Show($"Error on Check for Update : {response.StatusCode}");
                return false;
            }

        }

        public async static Task Update(string version)
        {
            string exePath = "C:\\Program Files (x86)\\ASN IT Inc\\POSpointe\\Updater\\MyPOSUpdater.exe";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = true, 
                Verb = "runas"
            };

            if (version != "Latest")
            {
                processStartInfo.Arguments = version;
            }

            try
            {
                await Task.Run(() => Process.Start(processStartInfo));
            }
            catch (Exception ex)
            {
                // Handle cases where the user denies admin access or other errors
                Console.WriteLine("Error: " + ex.Message);
            }
        }


        internal class TblVersion
        {
            public int Version { get; set; }

            public string? CurrentVersion { get; set; }
        }

    }
}
