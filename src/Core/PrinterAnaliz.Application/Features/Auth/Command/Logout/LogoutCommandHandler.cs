using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace PrinterAnaliz.Application.Features.Auth.Command.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommandRequest, LogoutCommandResponse>
    {
        private readonly IUserRepository rpUser;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;
        public LogoutCommandHandler(IUserRepository rpUser, ITokenService tokenService, IConfiguration configuration)
        {
            this.rpUser = rpUser;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }

        public async Task<LogoutCommandResponse> Handle(LogoutCommandRequest request, CancellationToken cancellationToken)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal is null)
                throw new ValidationException("Token bilgisine erişilemedi.");

            var strUserId = principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            _ = long.TryParse(strUserId ?? "0", out long userId);

            if (userId == 0)
                throw new ValidationException("Kullanıcı bilgisine erişilemedi.");
            var user = rpUser.Where(w => w.Id == userId && w.RefreshToken == request.RefreshToken && !w.IsDeleted).FirstOrDefault();
            if (user is null)
                throw new ValidationException("Kullanıcı datasına erişilemedi.");
            if (user.RefreshToken != request.RefreshToken)
                throw new ValidationException("Bu refresh token bu kullanıcı için geçerli değildir.");
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await rpUser.UpdateAsync(user);

            return new()
            {
                LoginStatus = false
            };
        }
    }
}
