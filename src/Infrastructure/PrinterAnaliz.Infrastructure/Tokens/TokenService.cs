using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrinterAnaliz.Application.Enums;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PrinterAnaliz.Infrastructure.Tokens
{
    public class TokenService : ITokenService
    { 
        private readonly TokenSettings tokenSettings;
        private readonly IHttpContextAccessor _accessor;

        public TokenService(IOptions<TokenSettings> options, IHttpContextAccessor accessor)
        {
            tokenSettings = options.Value;
            _accessor = accessor;
        }
        public async Task<JwtSecurityToken> CreateToken(User user, IList<UserRoles> userRoles, IList<long>? Customers)
        {
            var claims = new List<Claim>() 
            { 
             new Claim(JwtRegisteredClaimNames.Jti,user.Id.ToString()),
             new Claim(ClaimTypes.NameIdentifier,user.UserName),
             new Claim(ClaimTypes.Name,user.Name),
             new Claim(ClaimTypes.Surname,user.Surname),
             new Claim(ClaimTypes.Email,user.Email),
            };
            foreach (var role in userRoles) {
                UserRoleTypes roleName = (UserRoleTypes)role.RoleId;
                claims.Add(new Claim(ClaimTypes.Role, roleName.ToString()));
            }
            foreach (var customer in Customers)
            { 
                claims.Add(new Claim("Customers", customer.ToString()));
            }
            var sKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{tokenSettings.Secret}{DateTime.Now.ToString("yyyy-MM-dd")}"));
            var token = new JwtSecurityToken(
                issuer: tokenSettings.Issuer,
                audience: tokenSettings.Audience,
                expires: DateTime.Now.AddMinutes(tokenSettings.TokenValidityInMinutes),
                claims: claims,
                signingCredentials: new SigningCredentials(sKey, SecurityAlgorithms.HmacSha256)
                );
             
             
           return token;
        }

        public string GenerateRefreshToken()
        {
             var randomNumber = new byte[64];
             using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{tokenSettings.Secret}{DateTime.Now.ToString("yyyy-MM-dd")}")),
                ValidateLifetime = true
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Token bilgisi bulunamadı veya eşleşmedi.");
            return principal;
        }

        public ClaimsPrincipal? GetUserAccessor()=> _accessor?.HttpContext?.User;
        public LoginedUserModel GetLoginedUser()
        {
            LoginedUserModel result = new LoginedUserModel();
            var requestToken = _accessor?.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var principal =  GetPrincipalFromExpiredToken(requestToken);
             
            var strUserId = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            _ = long.TryParse(strUserId ?? "0", out long userId);
         
            result.Id = userId;
            result.userName = principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
            result.Name = principal.FindFirstValue(ClaimTypes.Name) ?? "UnName";
            result.Surname = principal.FindFirstValue(ClaimTypes.Surname) ?? "UnSurname";
            result.EmailEmail = principal.FindFirstValue(ClaimTypes.Email) ?? "UnEmail";
            result.UserRoles = new List<UserRoleTypes>();
            result.Customers = new List<long>();
            var userRoles = principal.FindAll(ClaimTypes.Role);
            var userCustomers = principal.FindAll("Customers");
            if (userCustomers != null)
            {
                foreach (var userCustomer in userCustomers)
                {
                    _ = long.TryParse(userCustomer.Value, out long cnvUserCustomer);
                    if (cnvUserCustomer>0)
                        result.Customers.Add(cnvUserCustomer);
                }
            }
            foreach (var role in userRoles)
            {
                
                if (Enum.TryParse(role.Value, out UserRoleTypes uRole))
                {
                    result.UserRoles.Add(uRole);
                }
            }
            return result;
        }
    }
}
