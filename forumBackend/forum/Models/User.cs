using System;
using System.Collections.Generic;

namespace forum.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? UserImage { get; set; }

    public string? Email { get; set; }

    public string? Sex { get; set; }

    public int? Phone { get; set; }

    public DateTime? CreationTime { get; set; }

    public string? Salt { get; set; }
}
