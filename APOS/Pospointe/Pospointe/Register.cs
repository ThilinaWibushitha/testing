using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe
{
    public class Register
    {
        public int regNo { get; set; }
        public string email { get; set; }
        public string businessName { get; set; }
        public string businessAddress { get; set; }
        public string registrationCode { get; set; }
        public string registerType { get; set; }
        public string active { get; set; }
        public string deviceMac { get; set; }
        public string deviceId { get; set; }
        public string dbMethod { get; set; }
        public string dbName { get; set; }
        public string portalPassword { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public object businesstype { get; set; }
        public object paymentMethod { get; set; }
        public object lastOtp { get; set; }
        public string runningVersion { get; set; }
        public bool uberEats { get; set; }
        public object uberEatsStoreId { get; set; }
        public object uberEatsToken { get; set; }
        public string localIp { get; set; }
        public string publicIp { get; set; }
        public string otpforStationConnection { get; set; }
        public bool loyaltyPlanStatus { get; set; }
        public object loyaltyStoreId { get; set; }
        public object loyaltyStoreGroupId { get; set; }
        public string agentID { get; set; }
        public string customerID { get; set; }
        public string token { get; set; }
        public DateTime? tokenExp { get; set; }
        public string industry { get; set; }
        public string storeId { get; set; }
        public string logoUrl { get; set; }
        public bool? onlineStore { get; set; }
        public bool advanceInventory { get; set; }
        public string? currency { get; set; }
        public string? country { get; set; }
        public bool? autoBatchClose { get; set; }
        public string? paxSn { get; set; }
        public string? mid { get; set; }
        public string? paxModel { get; set; }
        public string? paxApp { get; set; }
        public DateTime? lastSeen { get; set; }
        public bool? allowGiftCardProcessing { get; set; }
        public Guid? giftCardStoreId { get; set; }
        public bool? activateMarketPlace { get; set; }
        public Guid? marketPlaceDeviceId { get; set; }
        public string timecardStoreId { get; set; }
        public bool? isTimecardAllowed { get; set; }
        public bool? isQbAllowed { get; set; }
        public string realmId { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public int? accessTokenExpIn { get; set; }
        public int? refreshTokenExpIn { get; set; }
        public DateTime? accessTokenCreatedAt { get; set; }
        public DateTime? refreshTokenCreatedAt { get; set; }
        public string cardConnectMerchId { get; set; }
        public string cardConnectUsername { get; set; }
        public string cardConnectPassword { get; set; }
        public string cardConnectSite { get; set; }
        public string phoneNumber { get; set; }
        public string uberDirectCustomerId { get; set; }
        public bool? isPickup { get; set; }
        public bool? isDelivery { get; set; }
        public string baseUrl { get; set; }
    }
}
