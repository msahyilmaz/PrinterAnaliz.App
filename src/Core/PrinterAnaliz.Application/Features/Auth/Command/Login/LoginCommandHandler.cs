using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace PrinterAnaliz.Application.Features.Auth.Command.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {

        private readonly IUserRepository rpUser;
        private readonly IUserRolesRepository rpUserRoles;
        private readonly IUserCustomerRef rpUserCustomerRef;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;

        public LoginCommandHandler(IUserRepository rpUser, IUserRolesRepository rpUserRoles, ITokenService tokenService, IConfiguration configuration, IUserCustomerRef rpUserCustomerRef)
        {
            this.rpUser = rpUser;
            this.rpUserRoles = rpUserRoles;
            this.rpUserCustomerRef = rpUserCustomerRef;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }
        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {

            request.Password = PasswordEncryptor.Encrypt(request.Password);
            var qUser = await rpUser.FirstOrDefaultAsync(w => (w.UserName == request.UserName || w.Email == request.UserName) && w.Password == request.Password && !w.IsDeleted);
            if (qUser == null)
                throw new ValidationException("Kullanıcı bilgilerinizi kontrol ediniz.");

            var qUserRoles = await rpUserRoles.GetList(w => !w.IsDeleted && w.UserId == qUser.Id);
            if (qUserRoles == null)
                throw new ValidationException("Kullanıcı rolü bulunamadığı için giriş yapamaz.");

            var userCustomerIdList = rpUserCustomerRef.Where(w => w.UserId == qUser.Id && !w.IsDeleted).Select(w => w.CustomerId).ToList();

            JwtSecurityToken token = await tokenService.CreateToken(qUser, qUserRoles, userCustomerIdList);
            string strToken = new JwtSecurityTokenHandler().WriteToken(token);
            string strRefreshToken = tokenService.GenerateRefreshToken();
            _ = int.TryParse(configuration["JwtConfig:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);



            bool updateStatus = await UserLoginHandler(qUser, strRefreshToken, refreshTokenValidityInDays);
            if (updateStatus)
                return new() { AccessToken = strToken, RefreshToken = strRefreshToken, Expiration = token.ValidTo };
            else
                throw new ValidationException("Kullanıcı oturum bilgisi kaydedilemediği için oturum açılamadı.");
        }

        private async Task<bool> UserLoginHandler(User user,  string refreshToken, int refreshTokenValidityInDays)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
            return await rpUser.UpdateAsync(user) > 0 ? true : false;
        }
    }
}
