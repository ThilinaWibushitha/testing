using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe.Models
{
    public class Shiftcls
    {

        public string cashier { get; set; }
        public string shiftopen { get; set; }
        public string shiftclose { get; set; }
        public decimal startcash { get; set; }
        public decimal cashsales { get; set; }
        public decimal cashrefund { get; set; }
        public decimal expectedcash { get; set; }
        public decimal actualcash { get; set; }
        public decimal deference { get; set; }
        public decimal grosssales { get; set; }
        public decimal refunds { get; set; }
        public decimal discount { get; set; }
        public decimal tip { get; set; }
        public decimal totalcash { get; set; }
        public decimal totalcard { get; set; }
        public decimal totalcashrounding { get; set; }
        public TblDayOpenCashCollection tblDayOpenCashCollection { get; set; }
    }
}
