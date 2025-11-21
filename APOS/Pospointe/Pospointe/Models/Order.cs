using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public string? Platform { get; set; }

        public DateTime? DateTime { get; set; }

        public string? StoreId { get; set; }

        public string? OrderId { get; set; }

        public string? DeviceId { get; set; }

        public string? OrderData { get; set; }

        public string? Status { get; set; }

        public decimal? SubTotal { get; set; }

        public decimal? GrandTotal { get; set; }
        public string ProviderLogo { get; set; }
        public decimal? Tax { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? OrderShortId { get; set; }
    }
}
