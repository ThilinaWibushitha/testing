using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblUser
{
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserPin { get; set; } = null!;

    public string UserStatus { get; set; } = null!;

    public string? UserPicturePath { get; set; }

    public string UserPos { get; set; } = null!;

    public string UserBackEnd { get; set; } = null!;

    public string UserEndDayPeform { get; set; } = null!;

    public string UserDashBoard { get; set; } = null!;

    public bool? PerformEnddayForced { get; set; }

    public bool? RequestSupport { get; set; }

    public bool? LogtOut { get; set; }

    public bool? RecallInvoice { get; set; }

    public bool? RecallOldInvoice { get; set; }

    public bool? VoidTrans { get; set; }

    public bool? RetrnTrans { get; set; }

    public bool? CustomerManagement { get; set; }

    public bool? CreditCardSale { get; set; }

    public bool? CashSale { get; set; }

    public bool? GiftCardSale { get; set; }

    public bool? Allowpricechange { get; set; }

    public bool? Allowdiscount { get; set; }

    public bool? AllowReturns { get; set; }

    public bool? AllowCashDrawerOpen { get; set; }

    public bool? AllowGiftCardBalanceChk { get; set; }

    public bool? AllowNonTaxSales { get; set; }
}
