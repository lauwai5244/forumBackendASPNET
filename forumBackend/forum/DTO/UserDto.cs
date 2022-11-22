using System;
using System.Collections.Generic;
namespace forum.DTO;
public class UserDto
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}


public class UpdateUserDto
{
    public string Username { get; set; } = null!;

    public string OldPassword { get; set; } = null!;

    public string NewPassword { get; set; } = null!;
}

