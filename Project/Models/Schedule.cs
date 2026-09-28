using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Schedule
{
    public string ScheduleId { get; set; } = null!;

    public string TrainId { get; set; } = null!;

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public string? Direction { get; set; }

    public virtual Train Train { get; set; } = null!;
}
