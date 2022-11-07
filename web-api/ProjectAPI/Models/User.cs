using System.Text.Json.Serialization;

namespace ProjectAPI.Models;

public class User
{
    public double Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Greeting { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
}

