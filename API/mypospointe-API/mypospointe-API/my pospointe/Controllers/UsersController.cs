using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<TblUser> Get()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                return context.TblUsers.ToList();

            }
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public TblUser Get(string id)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
				var user = context.TblUsers.FirstOrDefault(item => item.UserId == id);
				if (user != null)
				{
					user.UserPin = CLSencryption.Decrypt(user.UserPin);
				}
				return user;

			}
		}

        // POST api/<UsersController>
        [HttpPost]
        public string Post([FromBody] TblUser value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context2 = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
               
                
                TblUser _user = new TblUser()
                {
                    UserId = value.UserId,
                    UserName = value.UserName,
					UserPin = CLSencryption.Encrypt(value.UserPin),
					UserStatus = value.UserStatus,
                    UserPicturePath = value.UserPicturePath,
                    UserPos = value.UserPos,
                    UserBackEnd = value.UserBackEnd,
                    UserEndDayPeform = value.UserEndDayPeform,
                    UserDashBoard = value.UserDashBoard,
                    PerformEnddayForced = value.PerformEnddayForced,
                    RequestSupport = value.RequestSupport,
                    LogtOut = value.LogtOut,
                    RecallInvoice = value.RecallInvoice,
                    RecallOldInvoice = value.RecallOldInvoice,
                    VoidTrans = value.VoidTrans,
                    RetrnTrans = value.RetrnTrans,
                    CustomerManagement = value.CustomerManagement,
                    CreditCardSale = value.CreditCardSale,
                    CashSale = value.CashSale,
                    GiftCardSale = value.GiftCardSale,
                    AllowCashDrawerOpen = value.AllowCashDrawerOpen,
                    Allowdiscount = value.Allowdiscount,
                    AllowReturns = value.AllowReturns,
                    Allowpricechange = value.Allowpricechange,
                    AllowGiftCardBalanceChk = value.AllowGiftCardBalanceChk,
                    AllowNonTaxSales = value.AllowNonTaxSales
                        


                };


                context2.TblUsers.Add(_user);
                context2.SaveChanges();
                return _user.UserId.ToString();
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] TblUser value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var item = context.TblUsers.FirstOrDefault(item => item.UserId == id);
                if (item != null)
                {


                    
                    item.UserName = value.UserName;
                    item.UserPin = CLSencryption.Encrypt(value.UserPin);
                    item.UserStatus = value.UserStatus;
                    item.UserPicturePath = value.UserPicturePath;
                    item.UserPos = value.UserPos;
                    item.UserBackEnd = value.UserBackEnd;
                    item.UserEndDayPeform = value.UserEndDayPeform;
                    item.UserDashBoard = value.UserDashBoard;
                    item.PerformEnddayForced = value.PerformEnddayForced;
                    item.RequestSupport = value.RequestSupport;
                    item.LogtOut = value.LogtOut;
                    item.RecallInvoice = value.RecallInvoice;
                    item.RecallOldInvoice = value.RecallOldInvoice;
                    item.VoidTrans = value.VoidTrans;
                    item.RetrnTrans = value.RetrnTrans;
                    item.CustomerManagement = value.CustomerManagement;
                    item.CreditCardSale = value.CreditCardSale;
                    item.CashSale = value.CashSale;
                    item.GiftCardSale = value.GiftCardSale;
                    item.AllowCashDrawerOpen = value.AllowCashDrawerOpen;
                    item.Allowdiscount = value.Allowdiscount;
                    item.AllowReturns = value.AllowReturns;
                    item.Allowpricechange = value.Allowpricechange;
                    item.AllowGiftCardBalanceChk = value.AllowGiftCardBalanceChk;
                    item.AllowNonTaxSales = value.AllowNonTaxSales;


                    context.SaveChanges();

                }


            }
        }

        // DELETE api/<UsersController>/5
       
    }
}
