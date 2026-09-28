using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Incident
{
    public string IncidentId { get; set; } = null!;

    public string? LineId { get; set; }

    public string? StationId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? ReportedTime { get; set; }

    public string? Severity { get; set; }

    public virtual Line? Line { get; set; }

    public virtual Station? Station { get; set; }
}
