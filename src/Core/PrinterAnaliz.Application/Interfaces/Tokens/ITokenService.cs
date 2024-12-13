using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PrinterAnaliz.Application.Interfaces.Tokens
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateToken(User user, IList<UserRoles> userRoles, IList<long>? Customers);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetUserAccessor();
        LoginedUserModel GetLoginedUser();
    }
}
