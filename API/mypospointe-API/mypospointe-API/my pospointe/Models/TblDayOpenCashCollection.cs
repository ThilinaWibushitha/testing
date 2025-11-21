using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblDayOpenCashCollection
{
    public int Id { get; set; }

    public int? DayOpenId { get; set; }

    public string? Type { get; set; }

    public int? Note100 { get; set; }

    public int? Note50 { get; set; }

    public int? Note20 { get; set; }

    public int? Note10 { get; set; }

    public int? Note5 { get; set; }

    public int? Note1 { get; set; }

    public int? Coin50 { get; set; }

    public int? Coin25 { get; set; }

    public int? Coin10 { get; set; }

    public int? Coin5 { get; set; }

    public int? Coin1 { get; set; }
}
