using HotelBookingSystem.API.Data;
using HotelBookingSystem.API.DTOs.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace HotelBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController :  ControllerBase
    {
    
        private readonly AppDbContext _db;
        
        public RoomController(AppDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _db.Rooms.Where(r => r.IsActive).Select(r => new RoomsReponse
            {
                Id = r.Id,
                Number = r.Number,
                Type = r.Type,
                PricePerNight = r.PricePerNight,
                Capacity = r.Capacity
            }).ToListAsync();

            return Ok(rooms);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(int id)
        {
            var room = await _db.Rooms.Where(r => r.Id == id && r.IsActive).Select(r => new RoomsReponse
            {
                Id = r.Id,
                Number = r.Number,
                Type = r.Type,
                PricePerNight = r.PricePerNight,
                Capacity = r.Capacity
            }).FirstOrDefaultAsync();

            if (room == null) return NotFound("Room Not Found");
            return Ok(room);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateRoomRequest request)
        {
            if (await _db.Rooms.AnyAsync(r => r.Number == request.Number))
            
                return BadRequest("Room Number Already Exists");

            var room = new Room
            {
                Number = request.Number,
                Type = request.Type,
                PricePerNight = request.PricePerNight,
                Capacity = request.Capacity,
                IsActive = true
            };

            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();
            return Ok($"Room {room.Number}Created Successfully ");

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>Update(int id , CreateRoomRequest request)
        {
            var room = await _db.Rooms.FindAsync(id);
            if (room == null) return NotFound("Room Not Found .");

            room.Number = request.Number;
            room.Type = request.Type;
            room.PricePerNight = request.PricePerNight;
            room.Capacity = request.Capacity;

            await _db.SaveChangesAsync();
            return Ok("Room Updated Successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult>Delete(int id)
        {
            var room = await _db.Rooms.FindAsync(id);
            if (room == null) return NotFound("Room Not Found");

            room.IsActive = false;
            await _db.SaveChangesAsync();
            return Ok("Room Deactivated SuccessFully");
        }


        [HttpGet("available")]
        public async Task<IActionResult>GetAvailable(DateTime checkin , DateTime checkout)
        {
            if (checkout <= checkin)
                return BadRequest("Check-out must be after check-in");

            var availableRooms = await _db.Rooms.Where(r => r.IsActive && !_db.Bookings
            .Any(b =>
            b.RoomId == r.Id &&
            b.Status != "Cancelled" &&
            b.CheckIn < checkout &&
            b.CheckOut > checkin
        ))
                .Select(r => new RoomsReponse
                {
                    Id = r.Id,
                    Number = r.Number,
                    Type = r.Type,
                    PricePerNight = r.PricePerNight,
                    Capacity = r.Capacity
                }).ToListAsync();

            return Ok(availableRooms);
        }
    }
        
    
}
