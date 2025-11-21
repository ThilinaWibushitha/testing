using Newtonsoft.Json;
using Pospointe.LocalData;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pospointe.Services
{

    class RegisterService
    {
        public static async Task RefreshRegister(string dbid)
        {

            var options = new RestClientOptions(clsConnections.baseurllogin)
            {
                Timeout = TimeSpan.FromSeconds(5),
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/POSLicense/{dbid}", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            if (response.IsSuccessful)
            {

                var objResponse1 = JsonConvert.DeserializeObject<Register>(response.Content);
                LoggedData.Reg = objResponse1;
                LoggedData.giftcardstoreid = objResponse1.giftCardStoreId.ToString();
                clsConnections.loyaltyactive = objResponse1.loyaltyPlanStatus;
                clsConnections.thistoreid = objResponse1.loyaltyStoreId?.ToString() ?? "";
                clsConnections.thisstoregroupid = objResponse1.loyaltyStoreGroupId?.ToString() ?? "";
                LoggedData.timcardstorestoreid = objResponse1.timecardStoreId;
                clsConnections.allowgiftcard = objResponse1.allowGiftCardProcessing ?? false;
                clsConnections.allowtimecard = objResponse1.isTimecardAllowed ?? false;
                // clsConnections.myposapiurl = objResponse1.baseUrl;

                // MessageBox.Show(objResponse1.baseUrl);
                //if (objResponse1.baseUrl == "https://api3.mypospointe.com:8843")
                //{
                //    //https://transserver2.mypospointe.com:9443
                //    clsConnections.transserverprimaryurl = "https://transserver2.mypospointe.com:9443";
                //    LoggedData.transbaseurl = clsConnections.transserverprimaryurl;
                //   // MessageBox.Show(clsConnections.transserverprimaryurl);
                //}
                if (objResponse1.country == "WEST")
                {
                    clsConnections.transserverprimaryurl = "https://transserver-west.azurewebsites.net";
                    LoggedData.transbaseurl = "https://transserver-west.azurewebsites.net";
                    clsConnections.myposapiurl = "https://mypospointeapi-west.azurewebsites.net";
                }
                UpdateMyPOSUSerAccess(objResponse1.regNo);
                SaveLicenseFile(objResponse1);
            }

        }

        private static void UpdateMyPOSUSerAccess(int regNo)
        {
            var options = new RestClientOptions(clsConnections.baseurllogin)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/MYPOSUsers/bydb/{regNo}", Method.Get);
            
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            if (response.IsSuccessful)
            {

                var objResponse1 = JsonConvert.DeserializeObject<MyPOSUser>(response.Content);
                LoggedData.myposuser = objResponse1;
            }
        }

        public static async Task SaveLicenseFile(Register reg)
        {
            // Serialize the object to JSON
            var json = JsonConvert.SerializeObject(reg);

            // Encode the JSON string to Base64
            var encryptedstring = CLSencryption.Encrypt(json);

            // Get the ProgramData path
            var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            // Define the POSpointe folder and file paths
            var posPointeFolder = System.IO.Path.Combine(programDataPath, "POSpointe");
            var licenseFilePath = System.IO.Path.Combine(posPointeFolder, "pospointe.lic");

            // Ensure the folder exists
            if (!Directory.Exists(posPointeFolder))
            {
                Directory.CreateDirectory(posPointeFolder);
            }

            // Save the encoded data to the file
            File.WriteAllText(licenseFilePath, encryptedstring);
        }

    }
}
