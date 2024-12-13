using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;

namespace PrinterAnaliz.Application.Features.Users.Commands.User.Delete
{
    public class DeleteUserCommandHandler  : IRequestHandler<DeleteUserCommandRequest, long>
    {


        private readonly IUserRepository rpUser;
        private readonly IUserRolesRepository rpUserRoles;
        private readonly IGenericAutoMapper mapper;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configration;

        public DeleteUserCommandHandler(IUserRepository rpUser, IUserRolesRepository rpUserRoles, IGenericAutoMapper mapper, ITokenService tokenService, IConfiguration configration)
        {
            this.rpUser = rpUser;
            this.rpUserRoles = rpUserRoles;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.configration = configration;
        }

        public async Task<long> Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
        {
             var existUser = await rpUser.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);
            if (existUser is null)
                throw new ValidationException("Silinecek kullanıcı bulunamadı.");

            var userRoles = await rpUserRoles.Where(w => w.UserId == request.Id).ToListAsync();

            if (userRoles is not null && userRoles.Any(a=>a.RoleId == 0))
                throw new ValidationException("Bu kullanıcıyı silmeye yetkiniz bulunmamaktadır. Sistem yöneticisi ile iletişime geçiniz.");

            if (existUser.ProfileImage != null)
            {
                var profileImagePath = configration.GetSection("UploadConfig").GetSection("UserProfileImagePath").Value;
                var profileImageUploadPath = Path.Combine(Directory.GetCurrentDirectory(), existUser.ProfileImage);
                if (File.Exists(profileImageUploadPath))
                    File.Delete(profileImageUploadPath);
            }

            var deletedUserRoles =await rpUserRoles.SoftDeleteRangeAsync(w=>w.UserId == request.Id);
            return await rpUser.SoftDeleteAsync(request.Id);
           }
    }
}
