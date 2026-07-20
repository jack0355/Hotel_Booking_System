public class Booking
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int GuestID {  get; set; }

    public DateTime CheckIn {  get; set; }
    public DateTime CheckOut { get; set; }
    public decimal TotalPrice {  get; set; }
    public string Status {  get; set; }
    public DateTime CreatedAt {  get; set; }


    public Room Room { get; set; }
    public Guest Guest { get; set; }

}
