using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Controllers;
using my_pospointe.Models;
using Newtonsoft.Json;
using RestSharp;
using Stripe.Forwarding;
using System.IO;
using System;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using RestSharp;
using Newtonsoft.Json;
using my_pospointe.Services;
using my_pospointe_api.Services;
using Microsoft.EntityFrameworkCore;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ShiftCloseNotification _shiftCloseNotification;
        private readonly SharedStateService _sharedStateService;

        public ShiftController(IConfiguration configuration, ShiftCloseNotification shiftCloseNotification, SharedStateService sharedStateService)
        {
            _configuration = configuration;
            _shiftCloseNotification = shiftCloseNotification;
            _sharedStateService = sharedStateService;
        }
        // GET: api/<ShiftController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ShiftController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        //Handle QuickBook OAuth
        [HttpGet("quickbook")]
        public IActionResult RedirectToQuickBooks([FromQuery] string DBNumber)
        {
            var connString = _configuration.GetRequiredSection("QuickBook");

            var baseUrl = connString["OAuthBaseURL"];
            var clientId = connString["ClientId"];
            var redirectUri = connString["RedirectUrl"];
            var scope1 = connString["Scope1"];
            var scope2 = connString["Scope2"];
            var state = DBNumber;

            var authUrl = $"{baseUrl}" +
                          $"?client_id={clientId}" +
                          $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                          $"&response_type=code" +
                          $"&scope={Uri.EscapeDataString(scope1)}" +
                          $" {Uri.EscapeDataString(scope2)}" +
                          $"&state={state}";

            return Redirect(authUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> QuickBooksCallback(string code, string realmId, string state)
        {
            var connString = _configuration.GetRequiredSection("QuickBook");

            string DBNumber = state;

            var FE_URL = connString["RedirectURLToFE"];

            var redirectURL = connString["RedirectURLToFE"];

            if (string.IsNullOrEmpty(DBNumber))
            {
                return BadRequest("DB Number is Missing");
            }

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(realmId))
            {
                return BadRequest("Missing code or realmId from the QuickBooks callback.");
            }


            QuickBooksTokenResponse qbResponse = await GetAccessToken(code);

            if(qbResponse != null)
            {
  
                var agentAPIBaseUrl = connString["AGENT_API_BASE_URL"];

                var getClient = new RestClient(agentAPIBaseUrl + $"/{DBNumber}");
                var getRequest = new RestRequest();

                getRequest.Method = Method.Get;

                var getResponse = await getClient.ExecuteAsync(getRequest);

                if (getResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    var qbUserDetails = JsonConvert.DeserializeObject<QBUserDetails>(getResponse.Content);

                    if (qbUserDetails != null && qbUserDetails.DBNumber == int.Parse(DBNumber)){

                        var qbDetails = new QBUserUpdateDetails
                        {
                            DBNumber = qbUserDetails.DBNumber,
                            Email = qbUserDetails.Email,
                            BusinessName = qbUserDetails.BusinessName,
                            BusinessAddress = qbUserDetails.BusinessAddress,
                            RegistrationCode = qbUserDetails.RegistrationCode,
                            RegisterType = qbUserDetails.RegisterType,
                            Active = qbUserDetails.Active,
                            DeviceMac = qbUserDetails.DeviceMac,
                            DeviceId = qbUserDetails.DeviceId,
                            DbMethod = qbUserDetails.DbMethod,
                            DbName = qbUserDetails.DbName,
                            PortalPassword = qbUserDetails.PortalPassword,
                            FirstName = qbUserDetails.FirstName,
                            LastName = qbUserDetails.LastName,
                            Businesstype = qbUserDetails.Businesstype,
                            PaymentMethod = qbUserDetails.PaymentMethod,
                            LastOtp = qbUserDetails.LastOtp,
                            UberEats = qbUserDetails.UberEats,
                            RunningVersion = qbUserDetails.RunningVersion,
                            UberEatsStoreId = qbUserDetails.UberEatsStoreId,
                            UberEatsToken = qbUserDetails.UberEatsToken,
                            LocalIp = qbUserDetails.LocalIp,
                            PublicIp = qbUserDetails.PublicIp,
                            OtpforStationConnection = qbUserDetails.OtpforStationConnection,
                            LoyaltyPlanStatus = qbUserDetails.LoyaltyPlanStatus,
                            LoyaltyStoreId = qbUserDetails.LoyaltyStoreId,
                            LoyaltyStoreGroupId = qbUserDetails.LoyaltyStoreGroupId,
                            AgentId = qbUserDetails.AgentId,
                            CustomerId = qbUserDetails.CustomerId,
                            Token = qbUserDetails.Token,
                            TokenExp = qbUserDetails.TokenExp,
                            Industry = qbUserDetails.Industry,
                            StoreId = qbUserDetails.StoreId,
                            LogoUrl = qbUserDetails.LogoUrl,
                            OnlineStore = qbUserDetails.OnlineStore,
                            AdvanceInventory = qbUserDetails.AdvanceInventory,
                            Currency = qbUserDetails.Currency,
                            Country = qbUserDetails.Country,
                            AutoBatchClose = qbUserDetails.AutoBatchClose,
                            PaxSn = qbUserDetails.PaxSn,
                            Mid = qbUserDetails.Mid,
                            PaxModel = qbUserDetails.PaxModel,
                            ActivateMarketPlace = qbUserDetails.ActivateMarketPlace,
                            MarketPlaceDeviceId = qbUserDetails.MarketPlaceDeviceId,
                            BusinessCity = qbUserDetails.BusinessCity,
                            BusinessState = qbUserDetails.BusinessState,
                            BusinessZipCode = qbUserDetails.BusinessZipCode,
                            TimecardStoreId = qbUserDetails.TimecardStoreId,
                            IsTimecardAllowed = qbUserDetails.IsTimecardAllowed,
                            AllowGiftCardProcessing = qbUserDetails.AllowGiftCardProcessing,
                            GiftCardStoreId = qbUserDetails.GiftCardStoreId,
                            LastSeen = qbUserDetails.LastSeen,
                            PaxApp = qbUserDetails.PaxApp,
                            isQbAllowed = true,
                            ///////////////////
                            RealmID = realmId,
                            AccessToken = qbResponse.AccessToken,
                            RefreshToken = qbResponse.RefreshToken,
                            AccessToken_Exp_In = qbResponse.ExpiresIn,
                            RefreshToken_Exp_In = qbResponse.RefreshExpiresIn,
                            AccessToken_CreatedAt = DateTime.UtcNow,
                            RefreshToken_CreatedAt = DateTime.UtcNow,
                        };

                        if(qbUserDetails != null)
                        {
                            var updateClient = new RestClient(agentAPIBaseUrl + $"/{DBNumber}");
                            var updateRequest = new RestRequest();

                            updateRequest.Method = Method.Put;

                            updateRequest.AddHeader("Content-Type", "application/json");

                            var updateBody = JsonConvert.SerializeObject(qbDetails);

                            updateRequest.AddStringBody(updateBody, DataFormat.Json);

                            var updateResponse = await updateClient.ExecuteAsync(updateRequest);
                            if (!updateResponse.IsSuccessful) {
                                return BadRequest("Token Details not Updated");
                            }

                            ReportsController reportsController = new ReportsController();

                            DateTime dateNow = DateTime.Now;
                            DateTime tenYearsAgo = dateNow.AddYears(-10);

                            var getOldReports = reportsController.GetForQB(tenYearsAgo, dateNow, DBNumber);

                            if(getOldReports == null) return BadRequest("Old Reports Not Found");

                            QBSalesReceiptModel qBSalesReceipt = new QBSalesReceiptModel
                            {
                                Amount = float.Parse(getOldReports.netsales.ToString()),
                                Description = "Opening Balance",
                                DetailType = "SalesItemLineDetail",
                                Id = DBNumber.ToString(),
                                LineNum = 1,
                                SalesItemLineDetail = new SalesItemLineDetail
                                {
                                    Qty = 1,
                                    UnitPrice = float.Parse(getOldReports.netsales.ToString()),
                                    ItemRef = new ItemRef
                                    {
                                        name = "Cash",
                                        value = "1"
                                    },
                                    TaxCodeRef = new TaxCodeRef
                                    {
                                        value = "TAX"
                                    }
                                }
                            };
                            
                            await PostSalesReceipt(int.Parse(DBNumber), qBSalesReceipt);

                        }
                    }
                }
                else
                {
                    return NotFound("Store Not Found");
                }

            }
            else
            {
                return BadRequest("Response is Null");
            }

            return Redirect(redirectURL);

        }

        [HttpPost("get-token")]
        public async Task<QuickBooksTokenResponse> GetAccessToken(string code)
        {
            var connString = _configuration.GetRequiredSection("QuickBook");

            var codeExchangeUrl = connString["OAuthCodeExchangeURL"];
            var clientId = connString["ClientId"];
            var clientSecret = connString["ClientSecret"];
            var redirectUri = connString["RedirectUrl"];
            var scope = connString["Scope"];
            var state = connString["State"];

            var client = new RestClient(codeExchangeUrl);
            var request = new RestRequest();

            request.Method = Method.Post;

            var authHeader = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            request.AddHeader("Authorization", $"Basic {authHeader}");

            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", redirectUri);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to get access token: {response.Content}");
            }

            var tokenResult= JsonConvert.DeserializeObject<QuickBooksTokenResponse>(response.Content);
            return tokenResult;

        }

        //Revoke Token
        [HttpPost("refresh-token")]
        public async Task<QuickBooksTokenResponse> RefreshToken (string refreshToken)
        {

            var connString = _configuration.GetRequiredSection("QuickBook");

            var tokenUrl = connString["OAuthCodeExchangeURL"];
            var clientId = connString["ClientId"];
            var clientSecret = connString["ClientSecret"];

            var client = new RestClient(tokenUrl);
            var request = new RestRequest();
            request.Method = Method.Post;

            var authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            request.AddHeader("Authorization", $"Basic {authHeader}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");

            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", refreshToken);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to refresh token: {response.Content}");
            }

            var tokenResult = JsonConvert.DeserializeObject<QuickBooksTokenResponse>(response.Content);
            return tokenResult;

        }

        //Shift Open
        [HttpPost("shiftopen")]
        public async Task<IActionResult> Postshiftopen([FromBody] TblDayOpen value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {

                var getBusinessInfo =  await context.TblBusinessInfos.FirstOrDefaultAsync();

                var dayOpeningTime = DateTime.Now.TimeOfDay;

                if (getBusinessInfo != null)
                {
                    var radarAuthHeader = _configuration.GetRequiredSection("Radar").GetValue<string>("Radar_Auth_Header");
                    var client = new RestClient(new RestClientOptions(clsConnections.Radar_BaseURL));
                    var request = new RestRequest($"/v1/geocode/forward?query={getBusinessInfo.CityStatezip}", Method.Get);
                    request.AddHeader("Authorization", radarAuthHeader);
                    RestResponse response = await client.ExecuteAsync(request);

                    if(response != null && response.IsSuccessful)
                    {
                        var radarResObj = JsonConvert.DeserializeObject<TimeZoneRadar>(response.Content);

                        if(radarResObj != null && !string.IsNullOrEmpty(radarResObj.Addresses[0].TimeZone.Id))
                        {
                            var currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(radarResObj.Addresses[0].TimeZone.Id);

                            DateTime currentUTCTime = DateTime.UtcNow;

                            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(currentUTCTime, currentTimeZone);

                            dayOpeningTime = currentTime.TimeOfDay;

                        }
                    }

                }

                TblDayOpen op = new TblDayOpen
                {

                    CashierId = value.CashierId,
                    Date = value.Date,
                    OpeningBalance = value.OpeningBalance,
                    ClosingBalance = value.ClosingBalance,
                    Deference = value.Deference,
                    Status = "Active",
                    OpenedDateTime = value.OpenedDateTime,
                    DayOpeningTime = dayOpeningTime

                };

                context.TblDayOpens.Add(op);
                int newid = op.DayOpenId;

                TblDayOpenCashCollection cashCollection = new TblDayOpenCashCollection
                {

                    DayOpenId = newid,
                    Note100 = 0,
                    Note50 = 0,
                    Note20 = 0,
                    Note10 = 0,
                    Note5 = 0,
                    Note1 = 0,
                    Coin50 = 0,
                    Coin25 = 0,
                    Coin5 = 0,
                    Coin1 = 0,
                    Coin10 = 0,
                    Type = "DAYOPEN"


                };
                context.TblDayOpenCashCollections.Add(cashCollection);
                context.SaveChanges();
                return Ok(op);
            }

        }


        //Get Current Shift
        [HttpGet("currentshift/{cashierid}")]
        public IActionResult GetCurrentShift(string cashierid)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var currentshift = context.TblDayOpens.Where(x => x.CashierId == cashierid && x.Status == "Active").FirstOrDefault();
                if (currentshift == null)
                {
                    return NotFound();
                }
                return Ok(currentshift);
            }

        }



        // POST api/<ShiftController>
        [HttpPost("shiftclose/{id}")]
        public async Task<IActionResult> Postshiftclose(int id,[FromBody] shiftcloserequest value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var item = context.TblDayOpens.FirstOrDefault(item => item.DayOpenId == id);
                if (item != null)
                {


                    item.ClosingBalance = value.closingbalance;
                    item.Deference = value.deference;
                    item.DayClosingTime = value.closingtime.TimeOfDay;
                    item.ClosedDateTime = value.closingdatetime;
                    item.Status = "Closed";

                    context.SaveChanges();

                    _sharedStateService.SetPrioritySync(id);



                    TblDayOpenCashCollection col = new TblDayOpenCashCollection
                    {
                        DayOpenId = id,
                        Note100 = value.cls.Note100,
                        Note50 = value.cls.Note50,
                        Note20 = value.cls.Note20,
                        Note10 = value.cls.Note10,
                        Note5 = value.cls.Note5,
                        Note1 = value.cls.Note1,
                        Coin50 = value.cls.Coin50,
                        Coin25 = value.cls.Coin25,
                        Coin10 = value.cls.Coin10,
                        Coin5 = value.cls.Coin5,
                        Coin1 = value.cls.Coin1,
                        Type = "DAYCLOSE"
                    };

                    context.TblDayOpenCashCollections.Add(col);
                    context.SaveChanges();



                    QBSalesReceiptModel qBSalesReceiptModel = new QBSalesReceiptModel
                    {
                        Amount = float.Parse(value.closingbalance.ToString()),
                        Description = "Shift Closing Balance",
                        DetailType = "Pospointe Shiftclose Receipt",
                        Id = Guid.NewGuid().ToString(),
                        LineNum = 1,
                        SalesItemLineDetail = new SalesItemLineDetail
                        {
                            Qty = 1,
                            UnitPrice = float.Parse(value.closingbalance.ToString()),
                            ItemRef = new ItemRef
                            {
                                name = "Cash",
                                value = "1"
                            },
                            TaxCodeRef = new TaxCodeRef
                            {
                                value = "TAX"
                            }
                        }
                    };

                    await PostSalesReceipt(int.Parse(dbvalue), qBSalesReceiptModel);

                    bool Success = await _shiftCloseNotification.SendNotification(id, dbvalue, "Shift Closed", $"Cashier({item.CashierId}) closed the shift with Cash: ${value.closingbalance.ToString("N2")}");

                   

                    return Ok("Success");
                }

                else
                {
                    return NotFound();
                }

            }
        }

        //POST Make SalesRecipt Request in QB
        [HttpPost("salesreceipt")]
        public async Task<IActionResult> PostSalesReceipt(int DBNumber, QBSalesReceiptModel receipt)
        {
            var connString = _configuration.GetRequiredSection("QuickBook");

            var agentAPIBaseUrl = connString["AGENT_API_BASE_URL"];
            var receiptBaseURL = connString["SalesReceiptBaseURL"];
            var version = connString["PostRequestVersion"];
            var path = connString["PostRequestPath"];

            if (DBNumber == 0) return BadRequest("DB Number is Missing");
            if (receipt == null) return BadRequest("Sales Receipt is Missing");

            var getClient = new RestClient(agentAPIBaseUrl + $"/{DBNumber}");
            var getRequest = new RestRequest();

            getRequest.Method = Method.Get;

            var getResponse = await getClient.ExecuteAsync(getRequest);

            if (getResponse == null || !getResponse.IsSuccessful || getResponse.StatusCode == System.Net.HttpStatusCode.NotFound) return NotFound("Store Not Found");

            var qbUserDetails = JsonConvert.DeserializeObject<QBUserDetails>(getResponse.Content);
            if (qbUserDetails == null || qbUserDetails.DBNumber != DBNumber) return NotFound("Store Not Found");

            if (!qbUserDetails.isQbAllowed.GetValueOrDefault()) return BadRequest("QuickBook is not Allowed for this Store");

            List<QBSalesReceiptModel>? salesReceipts = new List<QBSalesReceiptModel>();


            salesReceipts.Add(new QBSalesReceiptModel
            {
                Id = receipt.Id,
                Amount = receipt.Amount,
                Description = receipt.Description,
                DetailType = receipt.DetailType,
                LineNum = receipt.LineNum,
                SalesItemLineDetail = new SalesItemLineDetail
                {
                    Qty = receipt.SalesItemLineDetail.Qty,
                    UnitPrice = receipt.SalesItemLineDetail.UnitPrice,
                    ItemRef = new ItemRef
                    {
                        name = receipt.SalesItemLineDetail.ItemRef.name,
                        value = receipt.SalesItemLineDetail.ItemRef.value
                    },
                    TaxCodeRef = new TaxCodeRef
                    {
                        value = receipt.SalesItemLineDetail.TaxCodeRef.value
                    }
                }
            });

            if (salesReceipts.Count == 0) return BadRequest("Sales Receipt is Missing");

            var receiptPayload = new
            {
                Line = salesReceipts
            };

            string json = JsonConvert.SerializeObject(receiptPayload, Formatting.Indented);

            if (json == null) return BadRequest("Error in Json Convertion");

            var qbAccessToken = qbUserDetails.AccessToken;
            var qbRefreshToken = qbUserDetails.RefreshToken;
            var accessTokenExp = qbUserDetails.AccessToken_Exp_In;
            var refreshTokenExp = qbUserDetails.RefreshToken_Exp_In;
            var accessTokenCreatedAt = qbUserDetails.AccessToken_CreatedAt;
            var refreshTokenCreatedAt = qbUserDetails.RefreshToken_CreatedAt;

            if (qbAccessToken == null || qbRefreshToken == null || accessTokenExp == null || refreshTokenExp == null || accessTokenCreatedAt == null || refreshTokenCreatedAt == null)
            {
                return BadRequest("QuickBook Token is Missing");
            }

            var accesstokenExpireTime = accessTokenCreatedAt.Value.AddSeconds(accessTokenExp.Value);
            var refreshtokenExpireTime = refreshTokenCreatedAt.Value.AddSeconds(refreshTokenExp.Value);

            if (DateTime.UtcNow >= accesstokenExpireTime)
            {
                if (DateTime.UtcNow >= refreshtokenExpireTime)
                {
                    return BadRequest("Refresh Token Expired. Contact Store Owner.");
                }

                QuickBooksTokenResponse refreshAccessToken = await RefreshToken(qbRefreshToken);
                if (refreshAccessToken == null) return BadRequest("Refresh Token Failed");

                if (qbUserDetails != null && qbUserDetails.DBNumber == DBNumber)
                {

                    var qbDetails = new QBUserUpdateDetails
                    {
                        DBNumber = qbUserDetails.DBNumber,
                        Email = qbUserDetails.Email,
                        BusinessName = qbUserDetails.BusinessName,
                        BusinessAddress = qbUserDetails.BusinessAddress,
                        RegistrationCode = qbUserDetails.RegistrationCode,
                        RegisterType = qbUserDetails.RegisterType,
                        Active = qbUserDetails.Active,
                        DeviceMac = qbUserDetails.DeviceMac,
                        DeviceId = qbUserDetails.DeviceId,
                        DbMethod = qbUserDetails.DbMethod,
                        DbName = qbUserDetails.DbName,
                        PortalPassword = qbUserDetails.PortalPassword,
                        FirstName = qbUserDetails.FirstName,
                        LastName = qbUserDetails.LastName,
                        Businesstype = qbUserDetails.Businesstype,
                        PaymentMethod = qbUserDetails.PaymentMethod,
                        LastOtp = qbUserDetails.LastOtp,
                        UberEats = qbUserDetails.UberEats,
                        RunningVersion = qbUserDetails.RunningVersion,
                        UberEatsStoreId = qbUserDetails.UberEatsStoreId,
                        UberEatsToken = qbUserDetails.UberEatsToken,
                        LocalIp = qbUserDetails.LocalIp,
                        PublicIp = qbUserDetails.PublicIp,
                        OtpforStationConnection = qbUserDetails.OtpforStationConnection,
                        LoyaltyPlanStatus = qbUserDetails.LoyaltyPlanStatus,
                        LoyaltyStoreId = qbUserDetails.LoyaltyStoreId,
                        LoyaltyStoreGroupId = qbUserDetails.LoyaltyStoreGroupId,
                        AgentId = qbUserDetails.AgentId,
                        CustomerId = qbUserDetails.CustomerId,
                        Token = qbUserDetails.Token,
                        TokenExp = qbUserDetails.TokenExp,
                        Industry = qbUserDetails.Industry,
                        StoreId = qbUserDetails.StoreId,
                        LogoUrl = qbUserDetails.LogoUrl,
                        OnlineStore = qbUserDetails.OnlineStore,
                        AdvanceInventory = qbUserDetails.AdvanceInventory,
                        Currency = qbUserDetails.Currency,
                        Country = qbUserDetails.Country,
                        AutoBatchClose = qbUserDetails.AutoBatchClose,
                        PaxSn = qbUserDetails.PaxSn,
                        Mid = qbUserDetails.Mid,
                        PaxModel = qbUserDetails.PaxModel,
                        ActivateMarketPlace = qbUserDetails.ActivateMarketPlace,
                        MarketPlaceDeviceId = qbUserDetails.MarketPlaceDeviceId,
                        BusinessCity = qbUserDetails.BusinessCity,
                        BusinessState = qbUserDetails.BusinessState,
                        BusinessZipCode = qbUserDetails.BusinessZipCode,
                        TimecardStoreId = qbUserDetails.TimecardStoreId,
                        IsTimecardAllowed = qbUserDetails.IsTimecardAllowed,
                        AllowGiftCardProcessing = qbUserDetails.AllowGiftCardProcessing,
                        GiftCardStoreId = qbUserDetails.GiftCardStoreId,
                        LastSeen = qbUserDetails.LastSeen,
                        PaxApp = qbUserDetails.PaxApp,
                        isQbAllowed = true,
                        ///////////////////
                        RealmID = qbUserDetails.RealmID,
                        AccessToken = refreshAccessToken.AccessToken,
                        RefreshToken = refreshAccessToken.RefreshToken,
                        AccessToken_Exp_In = refreshAccessToken.ExpiresIn,
                        RefreshToken_Exp_In = refreshAccessToken.RefreshExpiresIn,
                        AccessToken_CreatedAt = DateTime.UtcNow,
                        RefreshToken_CreatedAt = DateTime.UtcNow,
                    };

                    if (qbUserDetails != null)
                    {
                        var updateClient = new RestClient(agentAPIBaseUrl + $"/{DBNumber}");
                        var updateRequest = new RestRequest();

                        updateRequest.Method = Method.Put;

                        updateRequest.AddHeader("Content-Type", "application/json");

                        var updateBody = JsonConvert.SerializeObject(qbDetails);

                        updateRequest.AddStringBody(updateBody, DataFormat.Json);

                        var updateResponse = await updateClient.ExecuteAsync(updateRequest);
                        if (!updateResponse.IsSuccessful)
                        {
                            return BadRequest("Token Details not Updated");
                        }
                    }

                    var client1 = new RestClient(receiptBaseURL + $"{version}{qbUserDetails.RealmID}{path}");
                    var request1 = new RestRequest();

                    request1.Method = Method.Post;
                    request1.AddHeader("Content-Type", "application/json");
                    request1.AddHeader("Authorization", $"Bearer {refreshAccessToken.AccessToken}");


                    request1.AddStringBody(json, DataFormat.Json);

                    var response1 = await client1.ExecuteAsync(request1);

                    if (!response1.IsSuccessful) return (BadRequest("Sales Receipt Request Failed"));

                    return Ok(response1.Content);

                }

            }
            
            var client = new RestClient(receiptBaseURL + $"{version}{qbUserDetails.RealmID}{path}");
            var request = new RestRequest();

            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {qbAccessToken}");

            request.AddStringBody(json, DataFormat.Json);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful) return (BadRequest("Sales Receipt Request Failed"));

            return Ok(response.Content);
        }

        // PUT api/<ShiftController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShiftController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public class shiftcloserequest()
        { 
            public decimal closingbalance { get; set; }
            public decimal deference { get; set; }

            public DateTime closingtime { get; set; }

            public DateTime closingdatetime { get; set; }

            public TblDayOpenCashCollection cls { get; set; }

        }

        public class shiftcloseWrapper()
        {
            public shiftcloserequest ShiftCloseRequest { get; set; }
            public QBSalesReceiptModel Receipt { get; set; }
        }

    }
}
