using Microsoft.AspNetCore.Mvc;
using my_pospointe.Models;
using Newtonsoft.Json.Linq;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {


                      //////////////////////////////////////////////////////////////////////////PURCHASE ORDER SUMMERY CONTROL//////////////////////////////////////////////////////////////////////////



        // GET: api/<AdvancedInventoryController>
        [HttpGet("{storeid}")]
        public IEnumerable<PosSummery> Get(Guid storeid)
        {
            using (var context = new PosavanceInventoryContext())
            {
                return context.PosSummeries
                .Where(x => x.StoreId == storeid).ToList();

            }
            
        }

        // GET api/<AdvancedInventoryController>/5
        [HttpGet("purchaseorders/{poid}")]
        public PosSummery GetPO(Guid poid)
        {
            using (var context = new PosavanceInventoryContext())
            {
                return context.PosSummeries.FirstOrDefault(item => item.Id == poid);

            }
        }

        // POST api/<AdvancedInventoryController>
        [HttpPost]
        public string Post([FromBody] PosSummery value)
        {
            using (var context2 = new PosavanceInventoryContext())
            {
                Guid id = Guid.NewGuid();
                PosSummery _item = new PosSummery()
                {

                    Id = id,
                    StoreId = value.StoreId,
                    CreatedDate = DateTime.Now,
                    CreatedPerson = value.CreatedPerson,
                    CreatedFrom = value.CreatedFrom,
                    Status = "Open",
                    LastUpdated = DateTime.Now,
                    SubCost = 0,
                    TotalCost = 0,
                    ShippingCost = 0,
                    VendorId = value.VendorId,
                    VenderName = value.VenderName,
                    SentEmails = value.SentEmails,
                    ExpDate = DateTime.Now.AddDays(30),
                    Notes = value.Notes


                };


                context2.PosSummeries.Add(_item);
                context2.SaveChanges();
                return _item.Id.ToString();
            }

        }

        // PUT api/<AdvancedInventoryController>/5
        [HttpPatch("{id}")]
        public void Put(Guid id, [FromBody] PosSummery value)
        {
            using (var context = new PosavanceInventoryContext())
            {
                var item = context.PosSummeries.FirstOrDefault(item => item.Id == id);
                if (item != null)
                {


                    item.Status= value.Status;
                    item.LastUpdated= DateTime.Now;
                    item.SubCost= value.SubCost;
                    item.TotalCost= value.TotalCost;
                    item.ShippingCost= value.ShippingCost;
                    item.SentEmails= value.SentEmails;
                    item.Notes= value.Notes;
                    context.SaveChanges();

                }


            }
        }

        // DELETE api/<AdvancedInventoryController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}





        //////////////////////////////////////////////////////////////////////////PURCHASE ORDER DETAILS CONTROL//////////////////////////////////////////////////////////////////////////


        [HttpGet("POItems/{poid}")]
        public IEnumerable<PoItem> GetPoItems(Guid poid)
        {
            using (var context = new PosavanceInventoryContext())
            {
                return context.PoItems
                .Where(x => x.Poid == poid).ToList();

            }

        }



        [HttpPost("POItems/{poid}")]
        public string PostItems([FromBody] PoItem value , Guid poid)
        {
            using (var context2 = new PosavanceInventoryContext())
            {
                
                PoItem _item = new PoItem()
                {
                    StoreId = value.StoreId,
                    Poid = poid,
                    ItemId = value.ItemId,
                    ItemName = value.ItemName,
                    CostPerItem = value.CostPerItem,
                    ItemInCase = value.ItemInCase,
                    CostPerCase = value.CostPerCase,
                    Qtyordered = value.Qtyordered,
                    Qtyreceived = 0,
                    Qtydamaged = 0,
                    Notes = value.Notes

                };


                context2.PoItems.Add(_item);
                context2.SaveChanges();
                return _item.Id.ToString();
            }

        }



        [HttpPatch("POItems/{id}")]
        public void EditItems(Guid id, [FromBody] PoItem value)
        {
            using (var context = new PosavanceInventoryContext())
            {
                var item = context.PoItems.FirstOrDefault(item => item.Id == id);
                if (item != null)
                {


                    item.CostPerItem = value.CostPerItem;
                    item.Qtyordered = value.Qtyordered;
                    item.Qtyreceived = value.Qtyreceived;
                    item.Qtydamaged = value.Qtydamaged;
                    item.Notes = value.Notes;
                    item.CostPerCase = value.CostPerCase; 
                    context.SaveChanges();

                }


            }
        }


        //DELETE api/<AdvancedInventoryController>/5
        [HttpDelete("POItems/{id}")]
        public void DeleteItem(Guid id)
        {
            using (var context2 = new PosavanceInventoryContext())
            {
                var item = context2.PoItems.FirstOrDefault(item => item.Id == id);
                if (item != null)
                {
                    context2.PoItems.Remove(item);
                    context2.SaveChanges();
                 
                }
            }

        }





    }
}
