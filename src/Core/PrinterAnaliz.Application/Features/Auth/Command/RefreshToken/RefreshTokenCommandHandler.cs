using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PrinterAnaliz.Application.Features.Auth.Command.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandRequest, RefreshTokenCommandResponse>
    {

        private readonly IUserRepository rpUser;
        private readonly IUserRolesRepository rpUserRoles;
        private readonly IUserCustomerRef rpUserCustomerRef;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration; 
        public RefreshTokenCommandHandler(IUserRepository rpUser, IUserRolesRepository rpUserRoles, ITokenService tokenService, IConfiguration configuration,IUserCustomerRef rpUserCustomerRef)
        {
            this.rpUser = rpUser;
            this.rpUserRoles = rpUserRoles;
            this.rpUserCustomerRef = rpUserCustomerRef;
            this.tokenService = tokenService;
            this.configuration = configuration; 
        }

        public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
        {
            var principal  = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal is null)
                throw new ValidationException("Token bilgisine erişilemedi.");
            var strUserId = principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
           _= long.TryParse(strUserId ?? "0", out long userId);
            if (userId == 0)
                throw new ValidationException("Kullanıcı bilgisine erişilemedi.");

            var user = rpUser.Where(w=>w.Id == userId && w.RefreshToken == request.RefreshToken && !w.IsDeleted).FirstOrDefault();
            var roles = rpUserRoles.Where(w=>w.UserId == userId && !w.IsDeleted).ToList();

            if (user is null)
                throw new ValidationException("Kullanıcı datasına erişilemedi.");
            if (user.RefreshToken != request.RefreshToken)
                throw new ValidationException("Bu refresh token bu kullanıcı için geçerli değildir.");

            if (user.RefreshTokenExpiryTime<= DateTime.Now)
                  throw new ValidationException("Oturum süresi sona ermiştir lütfen tekrar oturum açınız.");

            var userCustomerIdList = rpUserCustomerRef.Where(w => w.UserId == user.Id && !w.IsDeleted).Select(w => w.CustomerId).ToList();
            
            var newAccessToken = await tokenService.CreateToken(user!, roles, userCustomerIdList);
            var newRefreshToken = tokenService.GenerateRefreshToken();
          
            user.RefreshToken = newRefreshToken;
            _ = int.TryParse(configuration["JwtConfig:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
            await rpUser.UpdateAsync(user);

            return new()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken,
            };
        }
      
    }
}
