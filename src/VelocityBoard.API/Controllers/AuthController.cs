using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VelocityBoard.API.DTOs;
using VelocityBoard.API.Services;

namespace VelocityBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signIn;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signIn,TokenService tokenService)
        {
            _userManager = userManager;
            _signIn = signIn;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user != null)
                    return BadRequest("Email already exists");

                user = new IdentityUser
                {
                    UserName = dto.Email,
                    Email = dto.Email
                };

                var result = await _userManager.CreateAsync(user, dto.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                return Ok("User created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                    return Unauthorized("Invalid credentials");

                var result = await _signIn.CheckPasswordSignInAsync(user, dto.Password, false);
                if (!result.Succeeded)
                    return Unauthorized("Invalid credentials");

                var token = _tokenService.GenerateToken(user);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.Select(u => new
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToList();

            return Ok(users);
        }
    }
}
