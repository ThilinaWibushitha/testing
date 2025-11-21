using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblDayOpen
{
    public int DayOpenId { get; set; }

    public string CashierId { get; set; }

    public DateTime? Date { get; set; }

    public decimal? OpeningBalance { get; set; }

    public decimal? ClosingBalance { get; set; }

    public decimal? Deference { get; set; }

    public TimeSpan? DayOpeningTime { get; set; }

    public TimeSpan? DayClosingTime { get; set; }

    public string? Status { get; set; }

    public DateTime? OpenedDateTime { get; set; }

    public DateTime? ClosedDateTime { get; set; }
}
