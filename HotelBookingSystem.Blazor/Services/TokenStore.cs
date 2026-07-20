namespace HotelBookingSystem.Blazor.Services
{
    public static class TokenStore
    {
        public static string? AccessToken { get; set; }
        public static string? Role {  get; set; }

        public static bool IsLoggedIn=> !string.IsNullOrEmpty(AccessToken);
    }
}
