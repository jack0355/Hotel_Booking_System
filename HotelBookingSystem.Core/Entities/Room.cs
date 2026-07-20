public class Room
{
    public int Id {  get; set; }
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public decimal PricePerNight { get; set; }

    public int Capacity {  get; set; }

    public bool IsActive { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}