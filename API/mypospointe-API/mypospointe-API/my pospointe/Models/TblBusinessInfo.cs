using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblBusinessInfo
{
    public string StoreId { get; set; } = null!;

    public string? BusinessName { get; set; }

    public string? BusinessAddress { get; set; }

    public string? CityStatezip { get; set; }

    public string? BusinessPhone { get; set; }

    public string? BusinessEmail { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerPhone { get; set; }

    public string? LogoPath { get; set; }

    public string? Regcode { get; set; }

    public string? Footer1 { get; set; }

    public string? Footer2 { get; set; }

    public string? Footer3 { get; set; }

    public string? Footer4 { get; set; }

    public string? EncryptionKey { get; set; }

    public bool? AllowNegativeQty { get; set; }

    public bool? MixNmatch { get; set; }
}
