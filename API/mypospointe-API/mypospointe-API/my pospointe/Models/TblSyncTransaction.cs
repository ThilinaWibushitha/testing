using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace my_pospointe_api.Models;

public enum SyncStatus
{
    Pending,
    Uploading,
    Failed,
    Synced
}

[Table("TblSyncTransaction")]
public partial class TblSyncTransaction
{
    [Key]
    public long SyncId { get; set; }

    [Column(TypeName = "ntext")]
    public string TransactionData { get; set; }

    public SyncStatus Status { get; set; }

    public long ShiftId { get; set; }

    public int AttemptCount { get; set; }

    public DateTimeOffset LastAttemptTime { get; set; }
    
    [Column("CreatedDate", TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }
}