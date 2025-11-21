using Newtonsoft.Json;
using RestSharp;

namespace my_pospointe.Services
{
    public class ShiftCloseNotification
    {
        public async Task<bool> SendNotification(int DayOpenId, string dbName, string Title, string Body)
        {
            var options = new RestClientOptions()
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://asnitagentapi.azurewebsites.net/MobileDevices/SendCustomNotification", Method.Post);

            request.AddHeader("dbName", dbName);
            request.AddHeader("DayOpenId", DayOpenId.ToString());
            request.AddHeader("Content-Type", "application/json");
            var bodyObject = new
            {
                title = Title,
                body = Body
            };

            request.AddJsonBody(bodyObject);
            var response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            if (response.IsSuccessful)
            {
                Console.WriteLine(" Sent Successfully");
                return true;
            }
            else
            {
                Console.WriteLine($"Couldn't send:{response.Content} - {response.ContentType}");
                return false;
            }
        }
    }
}
