//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using TaskFlowBE.Data;
//using CoreEntities.Model;
//using System.Security.Cryptography;
//using System.Text;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.ComponentModel.DataAnnotations;

//namespace TaskFlowBE.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IConfiguration _config;

//        public AuthController(ApplicationDbContext context, IConfiguration config)
//        {
//            _context = context;
//            _config = config;
//        }

//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
//        {
//            // Kiểm tra username đã tồn tại chưa
//            if (await _context.Users.AnyAsync(u => u.FullName == request.Username))
//                return BadRequest("Username already exists");

//            // Mã hóa mật khẩu
//            string passwordHash = HashPassword(request.Password);

//            var user = new User
//            {
//                FullName = request.Username,
//                Email = request.Email,
//                PasswordHash = passwordHash
//            };

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            return Ok(new { message = "User registered successfully" });
//        }

//        private string HashPassword(string password)
//        {
//            using var sha = SHA256.Create();
//            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
//            return Convert.ToBase64String(bytes);
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
//        {
//            // Tìm user theo username
//            var user = await _context.Users.FirstOrDefaultAsync(u => u.FullName == request.Username);
//            if (user == null)
//                return BadRequest("Invalid username or password");

//            // Kiểm tra mật khẩu
//            var hash = HashPassword(request.Password);
//            if (hash != user.PasswordHash)
//                return BadRequest("Invalid username or password");

//            // Tạo JWT token
//            string token = CreateToken(user);
//            return Ok(new { token });
//        }

//        private string CreateToken(User user)
//        {
//            var claims = new List<Claim>
//    {
//        new Claim(ClaimTypes.Name, user.FullName),
//        new Claim(ClaimTypes.Email, user.Email),
//        new Claim("UserId", user.UserId.ToString())
//    };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: _config["Jwt:Issuer"],
//                audience: _config["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddHours(3),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//    }
//}
