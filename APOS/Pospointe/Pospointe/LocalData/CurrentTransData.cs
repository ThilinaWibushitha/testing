using Pospointe.Trans_Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pospointe.Trans_Api.TransData;

namespace Pospointe.LocalData
{
    public class CurrentTransData
    {
        public static TransData.Transmain MainData = new TransData.Transmain();

        public static List<Transitem> transitems = new List<Transitem>();

        public static decimal giftcardbalance = 0;
    }
}
