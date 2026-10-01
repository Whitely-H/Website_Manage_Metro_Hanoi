using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Passenger
{
    public string  PassengerId { get; set; }

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? IdentityCard { get; set; }

    public string? PassengerType { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<SmartCard> SmartCards { get; set; } = new List<SmartCard>();
}
