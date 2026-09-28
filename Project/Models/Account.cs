using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Account
{
    public Guid AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? StationId { get; set; }

    public virtual Station? Station { get; set; }
}
