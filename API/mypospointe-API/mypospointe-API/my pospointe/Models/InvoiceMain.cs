using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class InvoiceMain
{
    public Guid Id { get; set; }

    public Guid? FranchiseId { get; set; }

    public Guid? StoreId { get; set; }

    public string? InvoicePeriod { get; set; }

    public int InvoiceId { get; set; }

    public DateTime? Date { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? Tax1 { get; set; }

    public decimal? Tax2 { get; set; }

    public decimal? GrandTotal { get; set; }

    public string? TransType { get; set; }

    public string? Status { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CompletedDate { get; set; }

    public decimal? Discount { get; set; }

    public string? DiscountDecription { get; set; }

    public string? PaidAmount { get; set; }

    public string? BillInvoiceId { get; set; }

    public string? ManualPaymentType { get; set; }

    public string? BillPaymentId { get; set; }

    public string? BillBankId { get; set; }

    public string? BillCustomerId { get; set; }

    
}
