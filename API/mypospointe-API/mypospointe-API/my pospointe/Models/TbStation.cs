using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TbStation
{
    public int Id { get; set; }

    public string? StationName { get; set; }

    public string? Status { get; set; }

    public bool? OrderOnlyScreen { get; set; }

    public bool? ReceiptPrinter { get; set; }

    public string? Kotprinter1 { get; set; }

    public string? Kotprinter2 { get; set; }

    public bool? ChargeTax1 { get; set; }

    public bool? OpenCashDrawer { get; set; }

    public bool? PaxLogFile { get; set; }

    public int? ItemButtonsRows { get; set; }

    public int? ItemButtonsFontSize { get; set; }
}
