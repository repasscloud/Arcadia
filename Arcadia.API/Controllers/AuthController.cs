using Arcadia.API.Interfaces;
using Arcadia.Shared.Models.UserAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Arcadia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, IEmailSender emailSender)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            // Optionally, send confirmation email here

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { Status = "Error", Message = "Invalid authentication credentials." });

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new { Status = "Error", Message = "Invalid authentication credentials." });

            var token = GenerateJwtTokenAsync(user);

            return Ok(new { Token = token });
        }

        // POST: api/Auth/ResetPassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new { Status = "Error", Message = "User does not exist." });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action(nameof(ResetPasswordConfirm), "Auth", new { token, email = user.Email }, Request.Scheme);
            //var resetLink = Url.Action(nameof(ResetPasswordConfirm), "Auth", new { token, email = user.Email }, "https", Request.Host.Value);

            // Send email with reset link
            if (user.Email == null)
                return BadRequest(new { Status = "Error", Message = "Email address is blank." });
            await _emailSender.SendEmailAsync(user.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{resetLink}'>link</a>");

            return Ok(new { Status = "Success", Message = "Password reset link has been sent to your email." });
        }

        // GET: api/Auth/ResetPasswordConfirm
        [HttpGet("ResetPasswordConfirm")]
        public IActionResult ResetPasswordConfirm(string token, string email)
        {
            // This would typically be a frontend route
            // For API, you might handle password reset via a separate endpoint
            return Ok(new { Token = token, Email = email });
        }

        // POST: api/Auth/ConfirmResetPassword
        [HttpPost("ConfirmResetPassword")]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ConfirmResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new { Status = "Error", Message = "User does not exist." });

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
                return Error("Error resetting password.", StatusCodes.Status500InternalServerError);
                //return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Error resetting password." });

            return Ok(new { Status = "Success", Message = "Password has been reset successfully." });
        }

        // Helper method for formatting errors
        private IActionResult Error(string message, int statusCode = StatusCodes.Status500InternalServerError)
        {
            return StatusCode(statusCode, new { Status = "Error", Message = message });
        }

        // Helper method to generate JWT
        private async Task<string> GenerateJwtTokenAsync(IdentityUser user)
        {
            // Validate the user
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Email)) throw new ArgumentException("User email cannot be null or empty.");
            if (string.IsNullOrEmpty(user.Id)) throw new ArgumentException("User ID cannot be null or empty.");

            // Fetch JWT settings
            var jwtSettings = _configuration.GetSection("JwtSettings");
            if (jwtSettings == null) throw new InvalidOperationException("JwtSettings section is missing in configuration.");

            // Validate the required JWT settings
            var secret = jwtSettings["Secret"];
            if (string.IsNullOrEmpty(secret)) throw new InvalidOperationException("JwtSettings:Secret is missing in configuration.");
            var issuer = jwtSettings["Issuer"];
            if (string.IsNullOrEmpty(issuer)) throw new InvalidOperationException("JwtSettings:Issuer is missing in configuration.");
            var audience = jwtSettings["Audience"];
            if (string.IsNullOrEmpty(audience)) throw new InvalidOperationException("JwtSettings:Audience is missing in configuration.");
            var expirationInMinutes = jwtSettings["ExpirationInMinutes"];
            if (string.IsNullOrEmpty(expirationInMinutes) || !double.TryParse(expirationInMinutes, out double expiration))
                throw new InvalidOperationException("JwtSettings:ExpirationInMinutes is invalid or missing in configuration.");

            // Generate the signing key
            var key = Encoding.UTF8.GetBytes(secret);

            // Generate claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            // Generate the JWT token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
