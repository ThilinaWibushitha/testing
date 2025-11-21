using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using Newtonsoft.Json;
using System.Text;
using Pospointe.Models;
using System.Threading.Tasks;
using static Pospointe.MainWindow;
using System.Data;
using System.Windows;

namespace Pospointe.Services
{
    public class CloudDataSyncService
    {
        public async Task<bool> SyncCloudData(string dbName, Action<string> updateStatus = null)
        {
            try
            {
                updateStatus?.Invoke("Connecting to cloud...");

                var options = new RestClientOptions(clsConnections.myposapiurl)
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);
                var request = new RestRequest("/POS", Method.Get);
                request.AddHeader("db", dbName);

                var response = await client.ExecuteAsync(request);
                if (!response.IsSuccessful)
                {
                    updateStatus?.Invoke("Failed to fetch data from cloud.");
                    return false;
                }

                updateStatus?.Invoke("Downloading data...");

                var cloudData = JsonConvert.DeserializeObject<MainResponse>(response.Content);
                if (cloudData == null)
                {
                    updateStatus?.Invoke("Invalid response from server.");
                    return false;
                }

                //using (var context = new PosDb1Context())
                //{
                //    var users = context.TblUsers.ToList();
                //    var localItems = context.TblItems.ToList();
                //    var listOrderMapping = localItems.ToDictionary(item => item.ItemId, item => item.ListOrder);
                //    var departments = context.TblDepartments.ToList();
                //    var modifergroups = context.TblModiferGroups.ToList();
                //    var modiferitems = context.TblModifersofItems.ToList();
                //    var businessinfo = context.TblBusinessInfos.ToList();

                //    // Clear existing
                //    context.TblUsers.RemoveRange(users);
                //    context.TblItems.RemoveRange(localItems);
                //    context.TblDepartments.RemoveRange(departments);
                //    context.TblModiferGroups.RemoveRange(modifergroups);
                //    context.TblModifersofItems.RemoveRange(modiferitems);
                //    context.TblBusinessInfos.RemoveRange(businessinfo);

                //    // Add new
                //    context.TblUsers.AddRange(cloudData.users);
                //    context.TblDepartments.AddRange(cloudData.departments);
                //    context.TblModifersofItems.AddRange(cloudData.modifersofItems);
                //    context.TblModiferGroups.AddRange(cloudData.modiferGroups);
                //    context.TblBusinessInfos.AddRange(cloudData.businessInfo);

                //    foreach (var cloudItem in cloudData.items)
                //    {
                //        if (listOrderMapping.TryGetValue(cloudItem.ItemId, out var listOrderValue))
                //        {
                //            cloudItem.ListOrder = listOrderValue;
                //        }
                //        context.TblItems.Add(cloudItem);
                //    }

                //    await context.SaveChangesAsync();
                //}

                updateStatus?.Invoke("Updating license info...");
                await RegisterService.RefreshRegister(dbName);

              //  updateStatus?.Invoke("Getting Tax Rates...");
             //   await UpdateSalesTax(dbName);

                updateStatus?.Invoke("Cloud sync complete.");
                return true;
            }
            catch (Exception ex)
            {
                updateStatus?.Invoke($"Sync error: {ex.Message}");
                return false;
            }
        }

        private async Task UpdateSalesTax(string dbName)
        {
            var options = new RestClientOptions(clsConnections.myposapiurl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Business/taxrate", Method.Get);
            request.AddHeader("db", dbName);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            if (response.IsSuccessful)
            {
                TaxResponse myDeserializedClass = JsonConvert.DeserializeObject<TaxResponse>(response.Content);
                using (var context = new PosDb1Context())
                {
                    var tax = context.TblTaxRates.Where(x => x.TaxNo == 1).FirstOrDefault();
                    tax.TaxRate = Convert.ToDecimal(myDeserializedClass.taxRate ?? 0);
                    context.SaveChanges();

                }
            }
            else
            {
                MessageBox.Show("Error On Receiving Tax Rates.");
            }

        }

       
        public class TaxResponse
        {
            public int? taxNo { get; set; }
            public double? taxRate { get; set; }
        }
    }
}
