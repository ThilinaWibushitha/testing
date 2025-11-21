using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace my_pospointe.Models
{
    public class QBUserDetails
    {
        [JsonProperty("regNo")]
        public int? DBNumber { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("businessName")]
        public string? BusinessName { get; set; }

        [JsonProperty("businessAddress")]
        public string? BusinessAddress { get; set; }

        [JsonProperty("registrationCode")]
        public string? RegistrationCode { get; set; }

        [JsonProperty("registerType")]
        public string? RegisterType { get; set; }

        [JsonProperty("active")]
        public string? Active { get; set; }

        [JsonProperty("deviceMac")]
        public string? DeviceMac { get; set; }

        [JsonProperty("deviceId")]
        public string? DeviceId { get; set; }

        [JsonProperty("dbMethod")]
        public string? DbMethod { get; set; }

        [JsonProperty("dbName")]
        public string? DbName { get; set; }

        [JsonProperty("portalPassword")]
        public string? PortalPassword { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("businesstype")]
        public string? Businesstype { get; set; }

        [JsonProperty("paymentMethod")]
        public string? PaymentMethod { get; set; }

        [JsonProperty("lastOtp")]
        public string? LastOtp { get; set; }

        [JsonProperty("uberEats")]
        public bool? UberEats { get; set; }

        [JsonProperty("runningVersion")]
        public string? RunningVersion { get; set; }

        [JsonProperty("uberEatsStoreId")]
        public string? UberEatsStoreId { get; set; }

        [JsonProperty("uberEatsToken")]
        public string? UberEatsToken { get; set; }

        [JsonProperty("localIp")]
        public string? LocalIp { get; set; }

        [JsonProperty("publicIp")]
        public string? PublicIp { get; set; }

        [JsonProperty("otpforStationConnection")]
        public string? OtpforStationConnection { get; set; }

        [JsonProperty("loyaltyPlanStatus")]
        public bool? LoyaltyPlanStatus { get; set; }

        [JsonProperty("loyaltyStoreId")]
        public string? LoyaltyStoreId { get; set; }

        [JsonProperty("loyaltyStoreGroupId")]
        public string? LoyaltyStoreGroupId { get; set; }

        [JsonProperty("agentId")]
        public string? AgentId { get; set; }

        [JsonProperty("customerId")]
        public string? CustomerId { get; set; }

        [JsonProperty("token")]
        public Guid? Token { get; set; }

        [JsonProperty("tokenExp")]
        public DateTime? TokenExp { get; set; }

        [JsonProperty("industry")]
        public string? Industry { get; set; }

        [JsonProperty("storeId")]
        public Guid? StoreId { get; set; }

        [JsonProperty("logoUrl")]
        public string? LogoUrl { get; set; }

        [JsonProperty("onlineStore")]
        public bool? OnlineStore { get; set; }

        [JsonProperty("advanceInventory")]
        public bool? AdvanceInventory { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("autoBatchClose")]
        public bool? AutoBatchClose { get; set; }

        [JsonProperty("paxSn")]
        public string? PaxSn { get; set; }

        [JsonProperty("mid")]
        public string? Mid { get; set; }

        [JsonProperty("paxModel")]
        public string? PaxModel { get; set; }

        [JsonProperty("paxApp")]
        public string PaxApp { get; set; }

        [JsonProperty("lastSeen")]
        public DateTime? LastSeen { get; set; }

        [JsonProperty("allowGiftCardProcessing")]
        public bool? AllowGiftCardProcessing { get; set; }

        [JsonProperty("giftCardStoreId")]
        public Guid? GiftCardStoreId { get; set; }

        [JsonProperty("activateMarketPlace")]
        public bool? ActivateMarketPlace { get; set; }

        [JsonProperty("marketPlaceDeviceId")]
        public Guid? MarketPlaceDeviceId { get; set; }

        [JsonProperty("businessCity")]
        public string? BusinessCity { get; set; }

        [JsonProperty("businessState")]
        public string? BusinessState { get; set; }

        [JsonProperty("businessZipCode")]
        public string? BusinessZipCode { get; set; }

        [JsonProperty("timecardStoreId")]
        public string? TimecardStoreId { get; set; }

        [JsonProperty("isTimecardAllowed")]
        public bool? IsTimecardAllowed { get; set; }

        [JsonProperty("isQbAllowed")]
        public bool? isQbAllowed { get; set; }
        [JsonProperty("realmId")]
        public string? RealmID { get; set; }
        [JsonProperty("accessToken")]
        public string? AccessToken { get; set; }
        [JsonProperty("refreshToken")]
        public string? RefreshToken { get; set; }
        [JsonProperty("accessTokenExpIn")]
        public int? AccessToken_Exp_In { get; set; }
        [JsonProperty("refreshTokenExpIn")]
        public int? RefreshToken_Exp_In { get; set; }
        [JsonProperty("accessTokenCreatedAt")]
        public DateTime? AccessToken_CreatedAt { get; set; }
        [JsonProperty("refreshTokenCreatedAt")]
        public DateTime? RefreshToken_CreatedAt { get; set; }

    }
}
