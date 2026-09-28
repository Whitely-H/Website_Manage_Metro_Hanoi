using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TicketType
{
    public string TicketTypeId { get; set; } = null!;

    public string TypeName { get; set; } = null!;

    public decimal Price { get; set; }

    public int? DurationHours { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
