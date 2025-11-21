using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe
{
    public class POSOrderModel
    {

        public string platform { get; set; }
        public string orderid { get; set; }
        public string simpleorderid { get; set; }

        public string status { get; set; }
        public string ProviderLogo { get; set; }
        public string fulfillment_by { get; set; }
        public string storeid { get; set; }
        public DateTime? orderedDatetime { get; set; }
        public DateTime? FullfilmentDateTime { get; set; }
        public iCustomer icustomer { get; set; }

        public iTotal? iTotals { get; set; }
        public List<iItem>? iItems { get; set; }

        public string? orderacceptlink { get; set; }

        public string? live_order_management_url { get; set; }

        public string order_special_instructions { get; set; }

        public bool? is_plastic_ware_option_selected { get; set; }

    }

    public partial class iTotal
    {
        public double? SubTotal { get; set; }
        public double? TaxTotal { get; set; }
        public double GrandTotal { get; set; }
        public double? Discount { get; set; }
        public double? Tip { get; set; }
    }



    public partial class iCustomer
    {
        public string? Customerid { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public string? Phone { get; set; }
    }

    public class iItem
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public double? Price { get; set; }
        public int? Qty { get; set; }
        public string? Special_instructions { get; set; }

        public List<iModifers> Modifers { get; set; }

    }

    public class iModifers
    {
        public string GroupName { get; set; }

        public List<ModiferOptions> ModiferOptions { get; set; }

    }

    public class ModiferOptions
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public double? Price { get; set; }
        public int? Qty { get; set; }
        public object Quantity { get; internal set; }
    }
}
