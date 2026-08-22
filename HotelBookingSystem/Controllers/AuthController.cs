using HotelBookingSystem.API.Data;
using HotelBookingSystem.API.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;




namespace HotelBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }


        [HttpPost("register")]
        public async Task<IActionResult>Register(LoginRequest request)
        {
            if (await _db.Users.AnyAsync(u => u.username == request.Username))
                return BadRequest("The Name is already Taken");


            var user = new User
            {
                username = request.Username,
                passwordHash = HashPassword(request.Password),
                Role = "Guest"
            };
            user.SetPassword(request.Password);


            
            _db.Users.Add(user);




            await _db.SaveChangesAsync();


            return Ok("Registered Successfully ");
        }

        [HttpPost("Login")]
        public async Task<IActionResult>Login(LoginRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.username == request.Username);

            if (user == null || user.passwordHash != HashPassword(request.Password))
                return Unauthorized("Invalid Credentials");

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();


            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync();

            return Ok(new AuthReponse
            {
                AccessToken = accessToken,
                Refreshtoken = refreshToken,
                Role = user.Role
            });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]string refreshtoken)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u=> u.username ==  refreshtoken);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token");


            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync();

            return Ok(new AuthReponse
            {
                AccessToken = newAccessToken,
                Refreshtoken = newRefreshToken,
                Role = user.Role
            });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody]string Refreshtoken)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u=> u.username == Refreshtoken);
            if (user == null) return BadRequest();

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _db.SaveChangesAsync();
            return Ok("Logged Out");
        }


        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private string GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ??
                throw new InvalidOperationException("JWT key is missing") ));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier  , user.Id.ToString()),
                new Claim(ClaimTypes.Name , user.username),
                new Claim(ClaimTypes.Role , user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],         
                audience: _config["Jwt:Audience"],     
                claims: claims,
               expires: DateTime.UtcNow.AddMinutes(
               double.Parse(_config["Jwt:ExpiryMinutes"] ?? "60")), 
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
