namespace UserMgmt;
using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public Role Role { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;
}