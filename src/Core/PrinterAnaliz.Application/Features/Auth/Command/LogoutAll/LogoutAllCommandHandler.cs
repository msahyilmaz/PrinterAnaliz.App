using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace PrinterAnaliz.Application.Features.Auth.Command.LogoutAll
{
    public class LogoutAllCommandHandler : IRequestHandler<LogoutAllCommandRequest, LogoutAllCommandResponse>
    {
        private readonly IUserRepository rpUser;
        private readonly IUserRolesRepository rpUserRoles;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;
        public LogoutAllCommandHandler(IUserRepository rpUser, IUserRolesRepository rpUserRoles, ITokenService tokenService, IConfiguration configuration)
        {
            this.rpUser = rpUser;
            this.rpUserRoles = rpUserRoles;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }

        public async Task<LogoutAllCommandResponse> Handle(LogoutAllCommandRequest request, CancellationToken cancellationToken)
        {
            var loginedUser = tokenService.GetLoginedUser();
            if (loginedUser is null)
                throw new ValidationException("Token bilgisine erişilemedi.");

         
            if (loginedUser.Id == 0)
                throw new ValidationException("Kullanıcı bilgisine erişilemedi.");
          
            var user = rpUser.Where(w => w.Id == loginedUser.Id && w.RefreshToken == request.RefreshToken && !w.IsDeleted).FirstOrDefault();
            var userRoles = rpUserRoles.Where(w => w.Id == loginedUser.Id && !w.IsDeleted).ToList();
             
            if (user is null)
                throw new ValidationException("Kullanıcı datasına erişilemedi.");
            if (user.RefreshToken != request.RefreshToken)
                throw new ValidationException("Bu refresh token bu kullanıcı için geçerli değildir.");
            if (userRoles is not null && userRoles.Any(a => a.RoleId == 0))
                throw new ValidationException("Bu kullanıcıyı silmeye yetkiniz bulunmamaktadır. Sistem yöneticisi ile iletişime geçiniz.");

            var allUsers = rpUser.Where(w=>w.RefreshToken!=null && w.Id != loginedUser.Id).ToList();

            foreach (var logoutUser in allUsers)
            {
                logoutUser.RefreshToken = null;
                logoutUser.RefreshTokenExpiryTime = null;
                await rpUser.UpdateAsync(logoutUser);
            }

           
            return new()
            {
                LoginStatus = false
            };
        }
    }
}
