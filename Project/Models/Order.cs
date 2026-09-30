using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Order
{
    public string OrderId { get; set; } = null!;

    public string PassengerId { get; set; }

    public string TicketTypeId { get; set; } = null!;

    public DateTime? PurchaseDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Passenger? Passenger { get; set; }

    public virtual TicketType TicketType { get; set; } = null!;
}
