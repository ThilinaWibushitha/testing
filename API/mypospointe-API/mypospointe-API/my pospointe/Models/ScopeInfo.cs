using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class ScopeInfo
{
    public Guid SyncScopeId { get; set; }

    public string SyncScopeName { get; set; } = null!;

    public string? SyncScopeSchema { get; set; }

    public string? SyncScopeSetup { get; set; }

    public string? SyncScopeVersion { get; set; }

    public long? ScopeLastServerSyncTimestamp { get; set; }

    public long? ScopeLastSyncTimestamp { get; set; }

    public long? ScopeLastSyncDuration { get; set; }

    public DateTime? ScopeLastSync { get; set; }
}
