using Microsoft.AspNetCore.Mvc;
using my_pospointe.Models;
using Stripe.Climate;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        // GET: api/<ApplicatonsController>
        [HttpGet]
        public IEnumerable<FrApplication> Get()
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var stores = context.FrApplications.ToList();
                return stores;
            }
        }

        // GET api/<ApplicatonsController>/5
        [HttpGet("{id}")]
        public FrApplication Get(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                return context.FrApplications.FirstOrDefault(item => item.Id == id);
               
            }
        }

        [HttpGet("lead/{id}")]
        public IEnumerable<FrApplication> Getleadapps(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                return context.FrApplications.Where(item => item.AccountId == id).ToList();

            }
        }

        // POST api/<ApplicatonsController>
        [HttpPost]
        public IActionResult Post([FromBody] FrApplication value)
        {
            using (var context = new FranchiseManagementContext())
            {
                FrApplication _item = new FrApplication()
                {
                    AccountId = value.AccountId,
                    FirstName = value.FirstName,
                    LastName = value.LastName,
                    Email = value.Email,
                    Address = value.Address,
                    PrefPhoneNum = value.PrefPhoneNum,
                    NameOfCompany = value.NameOfCompany,
                    Dob = value.Dob,
                    Citizenship = value.Citizenship,
                    MartialStatus = value.MartialStatus,
                    NumberofDependents = value.NumberofDependents,
                    HowHeardUs = value.HowHeardUs,
                    WhenforMeeting = value.WhenforMeeting,
                    WhenforMeetingOth = value.WhenforMeetingOth,
                    FirstRestOpening = value.FirstRestOpening,
                    WhereOpenRest = value.WhereOpenRest,
                    Area1 = value.Area1,
                    Area2 = value.Area2,
                    Area3 = value.Area3,
                    WhyShahs = value.WhyShahs,
                    EmploymentStatus = value.EmploymentStatus,
                    CurrentEmployer = value.CurrentEmployer,
                    CurrentPosition = value.CurrentPosition,
                    EmployerLocation = value.EmployerLocation,
                    RecentEmplocation = value.RecentEmplocation,
                    RecentEmployer = value.RecentEmployer,
                    RecentPosition = value.RecentPosition,
                    HighEducLevel = value.HighEducLevel,
                    WillOtherParnter = value.WillOtherParnter,
                    PartnerNames = value.PartnerNames,
                    Cash = value.Cash,
                    MartableSec = value.MartableSec,
                    HomeValue = value.HomeValue,
                    OtherValue = value.OtherValue,
                    OtherAsset = value.OtherAsset,
                    LiqCapital = value.LiqCapital,
                    FinComment = value.FinComment,
                    AmofCrDb = value.AmofCrDb,
                    SoruceofDbCr = value.SoruceofDbCr,
                    FinancingtheBalance = value.FinancingtheBalance,
                    AssetComment = value.AssetComment,
                    MonthlySalery = value.MonthlySalery,
                    InSource1 = value.InSource1,
                    InSource2 = value.InSource2,
                    InSource3 = value.InSource3,
                    IncmComment = value.IncmComment,
                    SecuredNtspayble = value.SecuredNtspayble,
                    UnSecNtpayble = value.UnSecNtpayble,
                    AccntsPble = value.AccntsPble,
                    MortageDbt = value.MortageDbt,
                    OtherLible = value.OtherLible,
                    LibComnts = value.LibComnts,
                    RufrancOwner = value.RufrancOwner,
                    FranchiseHowNtype = value.FranchiseHowNtype,
                    Experiance = value.Experiance,
                    Questions = value.Questions
                   
                };


                context.FrApplications.Add(_item);
                context.SaveChanges();
                return  Ok(_item.Id.ToString());
            }

        }

        // PUT api/<ApplicatonsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApplicatonsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
