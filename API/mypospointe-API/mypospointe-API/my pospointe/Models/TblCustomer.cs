using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblCustomer
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? BusinessName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public string? CardNumber { get; set; }

    public decimal? DiscountRate { get; set; }

    public bool? TaxExempt { get; set; }

    public string? CardId { get; set; }

    public DateTime? CreatedDate { get; set; }
}
