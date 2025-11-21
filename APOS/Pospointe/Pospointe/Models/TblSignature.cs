using System;
using System.Collections.Generic;

namespace Pospointe.Models;

public partial class TblSignature
{
    public decimal TransMainId { get; set; }

    public byte[]? Signature { get; set; }
}
