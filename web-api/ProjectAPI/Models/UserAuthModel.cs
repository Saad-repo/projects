using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Models;

public class UserAuthModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}

public class UserAuthModelSignup
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}