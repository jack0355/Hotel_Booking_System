namespace HotelBookingSystem.API.DTOs.Auth
{
    public class AuthReponse
    {
        public string AccessToken {  get; set; }
        public string Refreshtoken { get; set; }
        public string Role {  get; set; }

    }
}
