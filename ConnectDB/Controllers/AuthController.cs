using ConnectDB.Data;
using ConnectDB.Helpers;
using ConnectDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConnectDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok("Đã login thành công");
        }

        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Thiếu username hoặc password");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == req.Username);

            if (user == null)
                return Unauthorized("Sai tài khoản");

            if (!PasswordHelper.Verify(req.Password, user.PasswordHash))
                return Unauthorized(new { message = "Sai mật khẩu" });

            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                return StatusCode(500, "JWT Key chưa cấu hình");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                username = user.Username,
                role = user.Role
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Thiếu username hoặc password");

            var exist = await _context.Users
                .AnyAsync(x => x.Username == req.Username);

            if (exist)
                return BadRequest("Username đã tồn tại");

            var user = new User
            {
                Username = req.Username,
                PasswordHash = PasswordHelper.Hash(req.Password),
                Role = "Staff"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Tạo tài khoản thành công");
        }
    }
}