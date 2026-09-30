
using System;
using System.Collections.Generic;
namespace Project.Models;
public partial class SmartCard
{
    public string CardId { get; set; } = null!;
    public string PassengerId { get; set; } = null!;
    public string NfcCode { get; set; } = null!;
    public decimal? Balance { get; set; }
    public DateTime? IssueDate { get; set; }
    public virtual Passenger Passenger { get; set; } = null!;
}