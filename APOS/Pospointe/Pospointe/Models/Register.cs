using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe.Models
{
    public class Register
    {
        public int regNo { get; set; }
        public string email { get; set; }
        public string businessName { get; set; }
        public string businessAddress { get; set; }
        public string registrationCode { get; set; }
        public string registerType { get; set; }
        public Guid? StoreId { get; set; }
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
        public bool? onlineStore { get; set; }
        public bool? advanceInventory { get; set; }
        public string? currency { get; set; }
        public string? country { get; set; }
        public bool? autoBatchClose { get; set; }
        public string? paxSn { get; set; }
        public string? mid { get; set; }
        public string? paxModel { get; set; }
        public string? paxApp { get; set; }
        public DateTime? lastSeen { get; set; }
        public bool? AllowGiftCardProcessing { get; set; }
        public Guid? GiftCardStoreId { get; set; }

    }
}
