public class User
{
    
    public int Id { get; set; }
    public string username {get; set; }

    public string passwordHash {get; set; }

    public string Role { get; set; }    

    public string? RefreshToken {  get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }
}
