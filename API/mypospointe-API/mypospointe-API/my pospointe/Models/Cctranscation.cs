using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class Cctranscation
{
    public Guid Id { get; set; }

    public string? MerchantService { get; set; }

    public Guid? FranchiseId { get; set; }

    public Guid? StoreId { get; set; }

    public Guid? InvoiceId { get; set; }

    public bool? PaidByAch { get; set; }

    public string? Respstat { get; set; }

    public string? Account { get; set; }

    public string? Token { get; set; }

    public string? Retref { get; set; }

    public string? Amount { get; set; }

    public string? Expiry { get; set; }

    public string? MerchId { get; set; }

    public string? Respcode { get; set; }

    public string? Resptext { get; set; }

    public string? Avsresp { get; set; }

    public string? Cvvresp { get; set; }

    public string? AuthCode { get; set; }

    public string? Respproc { get; set; }

    public string? Emv { get; set; }

    public string? Cardusestring { get; set; }

    public string? Subtype { get; set; }

    public string? Bin { get; set; }

    public DateTime? DateTime { get; set; }

    public string? SessionId { get; set; }

    public string? RedirectLink { get; set; }
}
