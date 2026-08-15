public class Review
{
    //Room Info
    public int Id { get; set; }
    public int BookingId { get; set; }


    //The Creating and the DateTime 
    public DateTime CreatedAt { get; set; }
    public Booking Booking { get; set; } = null!;
    //Rating and COmment Stuff 
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    
}