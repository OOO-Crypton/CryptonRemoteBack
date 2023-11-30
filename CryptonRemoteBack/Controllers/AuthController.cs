using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CryptonRemoteBack.Domain;
using CryptonRemoteBack.Extensions;
using CryptonRemoteBack.Model.Models;
using CryptonRemoteBack.Model.Views;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CryptonRemoteBack.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly int DaysForRefreshToken;
        private readonly int HoursForToken;
        private string UserId => User.GetUserId();

        public AuthController(ILogger<AuthController> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            DaysForRefreshToken = int.TryParse(_configuration["DaysForRefreshToken"], out int refreshToken)
                ? refreshToken
                : 7;
            HoursForToken = int.TryParse(_configuration["HoursForToken"], out int token)
                ? token
                : 24;
        }


        /// <summary>
        /// Вход в систему 
        /// </summary>
        /// <param name="data">Данные для входа</param>
        [HttpPost("/api/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel data)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(data.Email ?? "");
            if (user == null || !await _userManager.CheckPasswordAsync(user, data.Password ?? ""))
            {
                return Unauthorized(new { Email = "Введён неверный e-mail/пароль", Password = "" });
            }

            List<Claim>? authClaims = new()
            {
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim("UserId", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Email", user.Email ?? ""),
            };

            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (string userRole in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                authClaims.Add(new Claim("Role", userRole));
            }

            JwtSecurityToken? token = GetToken(authClaims, DateTime.UtcNow.AddHours(HoursForToken));

            string userRefreshToken = GenerateRefreshToken();
            user.RefreshToken = userRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(DaysForRefreshToken);
            await _userManager.UpdateAsync(user);

            Console.WriteLine($"User {data.Email} logged in");
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = userRefreshToken,
                expiration = token.ValidTo,
            });
        }


        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="registerModel">Данные для регистрации</param>
        [HttpPost("/api/register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterModel registerModel,
            CancellationToken ct)
        {
            if (await _userManager.FindByEmailAsync(registerModel.Email ?? "") != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User already exists");
            }

            ApplicationUser user = new()
            {
                Email = registerModel.Email,
                UserName = registerModel.UserName,
                PhoneNumber = registerModel.Phone,
            };
            IdentityResult? result = await _userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  string.Join(";\n", result.Errors.Select(x => x.Description)));
            }


            string role = UserRoles.User;
            if (!await _roleManager.RoleExistsAsync(role))
            {
                _ = await _roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"Role {role} was created");
            }
            _ = await _userManager.AddToRoleAsync(user, role);

            Console.WriteLine($"User {registerModel.Email} was created");
            return Ok("Success");
        }

        /// <summary>
        /// Обновление токена авторизации
        /// </summary>
        /// <param name="tokenModel">Сведения об обновляемом токене</param>
        [HttpGet("/api/refresh-token")]
        public async Task<IActionResult> RefreshToken([FromQuery] TokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;
            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("Invalid access token or refresh token (cannot be null or empty)");
            }

            ClaimsPrincipal? principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token");
            }

            string? username = principal.Identity?.Name;
            if (username == null)
            {
                return BadRequest("User not found in token");
            }

            ApplicationUser? user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("User not found in database");
            }
            if (user.RefreshToken != refreshToken)
            {
                return BadRequest("Invalid refresh token for user");
            }
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("Refresh token is expired");
            }


            JwtSecurityToken? newAccessToken = GetToken(principal.Claims.ToList(),
                                                        DateTime.UtcNow.AddHours(HoursForToken));

            string? newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            _ = await _userManager.UpdateAsync(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
                expiration = newAccessToken.ValidTo,
            });
        }


        /// <summary>
        /// Смена пароля
        /// </summary>
        /// <param name="model">Необходимые данные для смены пароля</param>
        [HttpPatch("/api/changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordModel model,
                                                        CancellationToken ct)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(UserId ?? "");
            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    return BadRequest("New password is empty.");
                }


                IdentityResult result = await _userManager
                    .ChangePasswordAsync(user, model.OldPassword ?? "", model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok("Password changed");
                }
                else
                {
                    string err = "";
                    foreach (IdentityError? error in result.Errors)
                    {
                        err += error.Description + "\n";
                    }
                    return BadRequest(err);
                }
            }

            return BadRequest($"User {UserId} not found in database");
        }


        /// <summary>
        /// Смена E-Mail адреса
        /// </summary>
        /// <param name="model">необходимые данные для смены адреса</param>
        [HttpPatch("/api/changeemail")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeUserEmailModel model,
                                                     CancellationToken ct)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(UserId ?? "");
            if (user != null)
            {
                if (!await _userManager.CheckPasswordAsync(user, model.Password ?? ""))
                {
                    return BadRequest(new { Password = "Wrong password" });
                }

                if (!string.IsNullOrWhiteSpace(model.Email))
                {
                    IdentityResult result = await _userManager
                        .SetEmailAsync(user, model.Email);

                    if (!result.Succeeded)
                    {
                        string err = "";
                        foreach (IdentityError? error in result.Errors)
                        {
                            err += error.Description + "\n";
                        }
                        return BadRequest(err);
                    }
                }

                return Ok("Email changed successfully");
            }

            return BadRequest($"User {UserId} not found in database");
        }


        /// <summary>
        /// Смена номера телефона
        /// </summary>
        /// <param name="model">Необходимые данные для смены номера телефона</param>
        [HttpPatch("/api/changephone")]
        [Authorize]
        public async Task<IActionResult> ChangePhone([FromBody] ChangeUserPhoneModel model,
                                                     CancellationToken ct)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(UserId ?? "");
            if (user != null)
            {
                if (!await _userManager.CheckPasswordAsync(user, model.Password ?? ""))
                {
                    return BadRequest(new { Password = "Wrong password" });
                }

                if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                {
                    IdentityResult result = await _userManager
                        .SetPhoneNumberAsync(user, model.PhoneNumber);

                    if (!result.Succeeded)
                    {
                        string err = "";
                        foreach (IdentityError? error in result.Errors)
                        {
                            err += error.Description + "\n";
                        }
                        return BadRequest(err);
                    }
                }

                return Ok("Phone number changed successfully");
            }

            return BadRequest($"User {UserId} not found in database");
        }


        /// <summary>
        /// Получение информации о текущем пользователе
        /// </summary>
        [HttpGet("/api/getuserinfo")]
        [Authorize]
        public async Task<ActionResult<UserView>> GetUserInfo(
            CancellationToken ct)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return BadRequest("Cant find " + UserId);
            }
            return Ok(new UserView(user));
        }

        #region privates

        private JwtSecurityToken GetToken(List<Claim> authClaims, DateTime validTo)
        {
            SymmetricSecurityKey? authSigningKey
                = new(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            JwtSecurityToken? token = new(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: validTo,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,
                                                           SecurityAlgorithms.HmacSha256));

            return token;
        }

        private static string GenerateRefreshToken()
        {
            byte[]? randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            TokenValidationParameters? tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler? tokenHandler = new();
            ClaimsPrincipal? principal = tokenHandler
                .ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            return securityToken is not JwtSecurityToken jwtSecurityToken
                    || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase)
                ? null
                : principal;
        }

        #endregion
    }
}
