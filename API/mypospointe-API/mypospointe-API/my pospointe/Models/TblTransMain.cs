using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace my_pospointe.Models;


public partial class TblTransMain
{
    public decimal InvoiceId { get; set; }

    public string? TransType { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Tax1 { get; set; }

    public decimal? GrandTotal { get; set; }

    public DateTime SaleDateTime { get; set; }

    public DateTime? SaleDate { get; set; }

    public TimeSpan? SaleTime { get; set; }

    public decimal? CashAmount { get; set; }

    public decimal? CardAmount { get; set; }

    public string? CardNumber { get; set; }

    public string? StationId { get; set; }

    public string? CashierId { get; set; }

    public decimal? CashChangeAmount { get; set; }

    public string? Paidby { get; set; }

    public string? Retref { get; set; }

    public string? CardType { get; set; }

    public string? CardHolder { get; set; }

    public decimal? InvoiceDiscount { get; set; }

    public string? PhoneNo { get; set; }

    public string? EntryMethod { get; set; }

    public string? AccountType { get; set; }

    public string? Aid { get; set; }

    public string? Tcarqc { get; set; }

    public string? Href { get; set; }

    public string? HostRefNum { get; set; }

    public string? DeviceOrgRefNum { get; set; }

    public string? CustomerId { get; set; }

    public decimal? TotalCredit { get; set; }

    public decimal? TipAmount { get; set; }

    public string? GiftCardNumber { get; set; }

    public string? CheckNumber { get; set; }

    public string? HoldName { get; set; }

    public string? CustomerName { get; set; }

    public decimal? LoyaltyDiscount { get; set; }
}
