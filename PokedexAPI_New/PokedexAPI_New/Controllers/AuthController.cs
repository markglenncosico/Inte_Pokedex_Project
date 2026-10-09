using Microsoft.AspNetCore.Mvc;
using PokedexAPI_New.Data;
using PokedexAPI_New.Models;
using System.Security.Cryptography;
using System.Text;

namespace PokedexAPI_New.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly PokedexContext _context;

        public AuthController(PokedexContext context)
        {
            _context = context;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username))
                return BadRequest("Username is required.");

            if (string.IsNullOrWhiteSpace(req.Password) || req.Password.Length < 6)
                return BadRequest("Password must be at least 6 characters.");

            if (_context.Users.Any(u => u.Username.ToLower() == req.Username.ToLower()))
                return BadRequest("That trainer name is already taken. Try a different one.");

            var user = new User
            {
                Username     = req.Username,
                Email        = req.Email ?? "",
                PasswordHash = HashPassword(req.Password),
                CreatedAt    = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Account created successfully!", username = user.Username });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Username and password are required.");

            var user = _context.Users
                .FirstOrDefault(u => u.Username.ToLower() == req.Username.ToLower());

            if (user == null || user.PasswordHash != HashPassword(req.Password))
                return Unauthorized("Invalid trainer name or password.");

            return Ok(new { message = "Login successful!", username = user.Username });
        }

        // ── Hash helper ──
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }

    // ── Request models ──
    public class RegisterRequest
    {
        public string Username { get; set; } = "";
        public string? Email   { get; set; }
        public string Password { get; set; } = "";
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
