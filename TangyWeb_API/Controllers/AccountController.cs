using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_Models;
using TangyWeb_API.Helpers;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly APISettings aPISettings;

        public AccountController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<APISettings> options)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            aPISettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO signUpRequestDTO)
        {
            if (signUpRequestDTO == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            // Create new AppicationUser as we registered ApplicationUser in program.cs
            var user = new ApplicationUser
            {
                Name = signUpRequestDTO.Name,
                Email = signUpRequestDTO.Email,
                UserName = signUpRequestDTO.Email,
                PhoneNumber = signUpRequestDTO.Phonenumber,
                EmailConfirmed = true
            };

            // Create that user.
            var result = await userManager.CreateAsync(user, signUpRequestDTO.Password);

            // If the result is no succeeded pass the SignUpResponseDTO with errors.
            if (!result.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO
                {
                    IsRegisterationSuccessful = false,
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            // Create role for this user.
            var roleResult = await userManager.AddToRoleAsync(user, StaticDetails.RoleCustomer);

            // If the result is no succeeded pass the SignUpResponseDTO with errors.
            if (!roleResult.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO
                {
                    IsRegisterationSuccessful = false,
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            // All done.
            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO signInRequestDTO)
        {
            if (signInRequestDTO == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await signInManager.PasswordSignInAsync(signInRequestDTO.UserName, signInRequestDTO.Password, true, false);

            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(signInRequestDTO.UserName);
                if (user == null)
                {
                    return Unauthorized(new SignInResponseDTO
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "User not found"
                    });
                }

                // Everything is valid need to login.
                var signingCredentials = GetSigningCredentials();
                var claims = await GetClaims(user);

                // Create jwt tokens.
                var jwtTokenOptions = new JwtSecurityToken
                    (
                        issuer: aPISettings.ValidIssuer,
                        audience: aPISettings.ValidAudience,
                        claims: claims,
                        signingCredentials: signingCredentials,
                        expires: DateTime.Now.AddDays(30)
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(jwtTokenOptions);

                return Ok(new SignInResponseDTO
                {
                    IsAuthSuccessful = true,
                    Token = token,
                    UserDTO = new()
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Id = user.Id,
                        PhoneNumber = user.PhoneNumber,
                    }
                });
            }
            else
            {
                return Unauthorized(new SignInResponseDTO
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid login request"
                });
            }
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(aPISettings.SecretKey!));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser applicationUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, applicationUser.Name!),
                new Claim(ClaimTypes.Email, applicationUser.Email!),
                new Claim("Id", applicationUser.Id)
            };

            var roles = await userManager.GetRolesAsync(await userManager.FindByNameAsync(applicationUser.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
