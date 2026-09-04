public class User
{
    
    public int Id { get; set; }
    public string username { get; set; } = string.Empty;

    public string passwordHash { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string? RefreshToken {  get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }



    public void SetPassword(string plainPassword)
    {
        passwordHash  = BCrypt.Net.BCrypt.HashPassword(plainPassword);
    }


    public bool VerifyPassword(string PlainPassword)
    {
        return BCrypt.Net.BCrypt.Verify(PlainPassword, passwordHash);
    }
}


