using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblProcessing
{
    public int StationId { get; set; }

    public string ProcessingMethod { get; set; } = null!;

    public string? Status { get; set; }

    public string? MerchantId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Auth { get; set; }

    public string? PaxIp { get; set; }

    public string? PaxPort { get; set; }

    public string? BusinessName { get; set; }

    public bool? SignatureCapture { get; set; }

    public string? SerialNum { get; set; }

    public string? AuthKey { get; set; }

    public bool? TipRequest { get; set; }
}
