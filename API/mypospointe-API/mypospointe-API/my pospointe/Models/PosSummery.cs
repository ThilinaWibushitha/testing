using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class PosSummery
{
    public Guid Id { get; set; }

    public int IdforStore { get; set; }

    public Guid? StoreId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedPerson { get; set; }

    public string? CreatedFrom { get; set; }

    public string? Status { get; set; }

    public DateTime? LastUpdated { get; set; }

    public decimal? SubCost { get; set; }

    public decimal? TotalCost { get; set; }

    public decimal? ShippingCost { get; set; }

    public string? VendorId { get; set; }

    public string? VenderName { get; set; }

    public string? SentEmails { get; set; }

    public DateTime? ExpDate { get; set; }

    public string? Notes { get; set; }
}
