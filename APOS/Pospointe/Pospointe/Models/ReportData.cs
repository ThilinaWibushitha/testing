using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe.Models
{
    public class ReportData
    {
        public string startdate { get; set; }
        public string enddate { get; set; }
        public double grosssales { get; set; }
        public double grosssaleswtax { get; set; }
        public double nontaxsales { get; set; }
        public double salestax { get; set; }
        public double netsales { get; set; }
        public double cashrefund { get; set; }
        public double cardrefund { get; set; }
        public double totalrefund { get; set; }
        public double cashtotal { get; set; }
        public double cardtotal { get; set; }
        public double giftcardtotal { get; set; }
        public double tiptotal { get; set; }
        public int nooftrans { get; set; }
        public int nooftransreturn { get; set; }
    }
}
