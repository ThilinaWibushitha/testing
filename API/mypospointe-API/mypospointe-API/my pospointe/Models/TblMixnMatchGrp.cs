using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblMixnMatchGrp
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? ExpireDate { get; set; }

    public decimal? RequiredQty { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedDate { get; set; }
}
