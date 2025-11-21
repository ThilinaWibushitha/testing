using System;
using System.Collections.Generic;

namespace Pospointe.Models;

public partial class TblDayOpen
{
    public int DayOpenId { get; set; }

    public string? CashierId { get; set; }

    public DateOnly? Date { get; set; }

    public decimal OpeningBalance { get; set; }

    public decimal ClosingBalance { get; set; }

    public decimal? Deference { get; set; }

    public TimeOnly? DayOpeningTime { get; set; }

    public TimeOnly? DayClosingTime { get; set; }

    public string? Status { get; set; }

    public DateTime OpenedDateTime { get; set; }

    public DateTime? ClosedDateTime { get; set; }
}
