using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblBusinessInfoTracking
{
    public string StoreId { get; set; } = null!;

    public Guid? UpdateScopeId { get; set; }

    public byte[]? Timestamp { get; set; }

    public long? TimestampBigint { get; set; }

    public bool SyncRowIsTombstone { get; set; }

    public DateTime? LastChangeDatetime { get; set; }
}
