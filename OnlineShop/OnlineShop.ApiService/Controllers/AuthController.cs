using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.ApiService.Data;
using OnlineShop.ApiService.Models;
using OnlineShop.ApiService.Services;
using OnlineShop.Shared.Constants;
using OnlineShop.Shared.Models;

namespace OnlineShop.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly UserDbContext _userDbContext;
        private IConfiguration _config;
        private ILogger<AuthController> _logger;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, TokenService tokenService, ILogger<AuthController> logger, UserDbContext userDbContext)
        {
            _config = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
            _userDbContext = userDbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                //check password pattern
                bool isPasswordOK = Regex.IsMatch(model.Password, WebConstants.PasswordRegexPattern);
                if (!isPasswordOK) return BadRequest("Must contain at least one number and one uppercase and lowercase letter, and at least 8 or more characters");

                //create user
                var userId = Guid.NewGuid().ToString();
                var user = new ApplicationUser { UserId = userId, Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errorDescriptions = result.Errors.Select(x => x.Description).ToList();
                    return BadRequest(String.Join(", ", errorDescriptions));
                }

                //give user role
                if (!await _userManager.IsInRoleAsync(user, "User"))
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }

                try
                {
                    //create customer
                    var userCustomer = new Customer()
                    {
                        ApplicationUser = user,
                        FirstName = "",
                        LastName = "",
                        PhoneNumber = "",
                        Cart = new Cart(),
                        ShippingAddress = new ShippingAddress() { City = "", Country = "", HouseNum = "", State = "", Street = "", ZipCode = "" },
                        BillingAddress = new BillingAddress() { City = "", Country = "", HouseNum = "", State = "", Street = "", ZipCode = "" }
                    };

                    _userDbContext.Add(userCustomer);
                    _userDbContext.SaveChanges();
                }
                catch (Exception)
                {
                    await _userManager.DeleteAsync(user);
                    throw;
                }
                

                return Ok(new { message = "User registered successfully!" });

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
                var userLocked = await _userManager.IsLockedOutAsync(user);
                if (userLocked) return Unauthorized("The requesting user is locked.");

                var roles = await _userManager.GetRolesAsync(user);

                var customer = _userDbContext.Customers.FirstOrDefault(x => x.ApplicationUser.UserId == user.UserId);

                var token = _tokenService.GenerateToken(user.UserId, customer.Id, user.Email, roles);

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
