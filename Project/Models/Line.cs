using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Line
{
    public string LineID { get; set; } = null!;

    public string LineName { get; set; } = null!;

    public string? ColorCode { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();

    public virtual ICollection<LineStation> LineStations { get; set; } = new List<LineStation>();

    public virtual ICollection<Train> Trains { get; set; } = new List<Train>();
}
