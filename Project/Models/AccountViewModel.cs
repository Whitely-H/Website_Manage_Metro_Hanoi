namespace Project.Models;
using System;
using System.Collections.Generic;

public class AccountCreateViewModel
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "";
    public string? StationId { get; set; }
}

public class AccountEditViewModel
{
    public Guid AccountId { get; set; }
    public string Username { get; set; } = "";
    public string? Password { get; set; }
    public string Role { get; set; } = "";
    public string? StationId { get; set; }
}