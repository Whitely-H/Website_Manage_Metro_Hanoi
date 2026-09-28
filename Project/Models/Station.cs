using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Station
{
    public string StationId { get; set; } = null!;

    public string StationName { get; set; } = null!;

    public string? Address { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();

    public virtual ICollection<LineStation> LineStations { get; set; } = new List<LineStation>();
}
