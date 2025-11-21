using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe
{
    public class MyPOSUser
    {
        
            public int? id { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public string accessibleDb { get; set; }
            public string userType { get; set; }
            public int? status { get; set; }
            public object lastOtp { get; set; }
            public string token { get; set; }
            public DateTime? tokenExp { get; set; }
            public bool? dashboardonly { get; set; }
            public bool? mobileappview { get; set; }
      
    }
}
