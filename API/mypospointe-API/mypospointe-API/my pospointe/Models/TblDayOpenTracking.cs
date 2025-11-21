using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblDayOpenTracking
{
    public int DayOpenId { get; set; }

    public Guid? UpdateScopeId { get; set; }

    public byte[]? Timestamp { get; set; }

    public long? TimestampBigint { get; set; }

    public bool SyncRowIsTombstone { get; set; }

    public DateTime? LastChangeDatetime { get; set; }
}
