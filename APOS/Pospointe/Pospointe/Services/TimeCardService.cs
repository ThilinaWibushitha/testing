using Microsoft.Data.Tools.Schema.Tasks.Sql;
using Newtonsoft.Json;
using Pospointe.LocalData;
using Pospointe.LoginWindow;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pospointe.Services
{
    class TimeCardService
    {
        public static async Task GetUserData(int userid , string pin)
        {
            TimecardUSerReq req = new TimecardUSerReq
            { 
             id = userid,
             password = pin
            };

            var json = JsonConvert.SerializeObject(req);

            var options = new RestClientOptions(clsConnections.timecardserver)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/EmployeeLog/{LoggedData.timcardstorestoreid}", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", clsConnections.timecardserverauth);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                TimeCardUserResp myDeserializedClass = JsonConvert.DeserializeObject<TimeCardUserResp>(response.Content);

                //MessageBox.Show($"Welcome Back {myDeserializedClass.employee.firstName} {myDeserializedClass.employee.lastName}");
                if (myDeserializedClass?.employee != null)
                {
                    // Open the Timecard Options window instead of MessageBox
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        FrmTimecardOptions timecardWindow = new FrmTimecardOptions(myDeserializedClass);
                        timecardWindow.ShowDialog();
                    });
                }

            }

            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show($"{response.Content}");
            }

            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show($"Response Code {response.StatusCode} Content : {response.Content}");
            }
        }


        public static string UpdateData(int punchtype, Guid empoloyeeid)
        {

            PungLogReq req = new PungLogReq();
            req.EmpId =  empoloyeeid;
            req.AuthMethod = "CARD";
            req.PosDevice = clsConnections.mydb;
            req.PosLocation = LoggedData.BusinessInfo.BusinessName;
            req.Message = "";
            req.StoreId = Guid.Parse(LoggedData.timcardstorestoreid);
            req.Id = 0;
            req.TimeStamp = DateTime.Now;
            req.PunchType = punchtype;


            var json = JsonConvert.SerializeObject(req);


            var options = new RestClientOptions(clsConnections.timecardserver)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/EmployeeLog", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", clsConnections.timecardserverauth);

            request.AddParameter("application/json", json, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            if (response.IsSuccessful)
            {
                return $"success";
            }

            else
            {
                return $"Error Code : {response.StatusCode} Message : {response.Content}";
            }
        }
    }


    

    public class TimecardUSerReq
    {
        public int? id { get; set; }
        public string password { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class PungLogReq
    {
        public Guid LogId { get; set; }

        public Guid? EmpId { get; set; }

        public string? AuthMethod { get; set; }

        public string? PosDevice { get; set; }

        public string? PosLocation { get; set; }

        public string? Message { get; set; }

        public Guid? StoreId { get; set; }

        public int? Id { get; set; }

        public DateTime? TimeStamp { get; set; }

        public int? PunchType { get; set; }
        
    }

    public class TimecardEmployee
    {
        public Guid empId { get; set; }
        public string storeId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool? status { get; set; }
        public string deptId { get; set; }
        public string phoneNo { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public DateTime? hireDate { get; set; }
        public string employmentType { get; set; }
        public int? id { get; set; }
        public string password { get; set; }
    }

    public class TimecardLastActivity
    {
        public string logId { get; set; }
        public string empId { get; set; }
        public string authMethod { get; set; }
        public object posDevice { get; set; }
        public object posLocation { get; set; }
        public object message { get; set; }
        public string storeId { get; set; }
        public int? id { get; set; }
        public DateTime timeStamp { get; set; }
        public int? punchType { get; set; }
    }

    public class TimeCardUserResp
    {
        public TimecardEmployee employee { get; set; }
        public TimecardLastActivity? lastActivity { get; set; }
    }
}
