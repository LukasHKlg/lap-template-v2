using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.ApiService.Models;
using OnlineShop.ApiService.Services;

namespace OnlineShop.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private IConfiguration _config;
        private ILogger<AuthController> _logger;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, TokenService tokenService, ILogger<AuthController> logger)
        {
            _config = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                var userId = Guid.NewGuid().ToString();
                var user = new ApplicationUser { UserId = userId, UserName = model.Username, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, CustomerPhone = model.CustomerPhone };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok(new { message = "User registered successfully!" });
                }
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while creating user: " + ex.ToString());
                return BadRequest("Sorry there has been an error while creating your user.");
                throw;
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                    return Unauthorized();

                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.GenerateToken(user.UserId, user.Email, roles);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while logging into user: " + ex.ToString());
                return BadRequest("Sorry there has been an error while logging into your user.");
                throw;
            }
            
        }
    }
}
