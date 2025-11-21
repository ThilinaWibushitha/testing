using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblDepartment
{
    public string DeptId { get; set; } = null!;

    public string DeptName { get; set; } = null!;

    public string? PicturePath { get; set; }

    public string Visible { get; set; } = null!;

    public string? BtnColor { get; set; }

    public string? NameVisible { get; set; }

    public string? PictureVisible { get; set; }

    public int? ListOrder { get; set; }

    public bool? ShowinOrderTablet { get; set; }
}
