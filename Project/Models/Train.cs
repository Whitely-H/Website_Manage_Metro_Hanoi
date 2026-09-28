using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Train
{
    public string TrainId { get; set; } = null!;

    public string TrainCode { get; set; } = null!;

    public string LineId { get; set; } = null!;

    public int Capacity { get; set; }

    public string? Status { get; set; }

    public virtual Line Line { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
