using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class SmartCard
{
    public Guid CardId { get; set; }

    public Guid PassengerId { get; set; }

    public string NfcCode { get; set; } = null!;

    public decimal? Balance { get; set; }

    public DateTime? IssueDate { get; set; }

    public virtual Passenger Passenger { get; set; } = null!;
}
