using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class LineStation
{
    public string LineId { get; set; } = null!;

    public string StationId { get; set; } = null!;

    public int OrderIndex { get; set; }

    public decimal? DistanceToNext { get; set; }

    public virtual Line Line { get; set; } = null!;

    public virtual Station Station { get; set; } = null!;
}
