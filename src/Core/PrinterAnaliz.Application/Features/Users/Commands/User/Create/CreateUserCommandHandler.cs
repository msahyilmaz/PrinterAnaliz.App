using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.Users.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, long>
    {
        private readonly IUserRepository rpUser;
        private readonly IUserRolesRepository rpUserRoles;
        private readonly IUserCustomerRef rpUserCustomerRef;
        private readonly IGenericAutoMapper mapper; 
        private readonly IConfiguration configration;

        public CreateUserCommandHandler(IUserRepository rpUser, IUserRolesRepository rpUserRoles, IGenericAutoMapper mapper, IConfiguration configration, IUserCustomerRef rpUserCustomerRef)
        {
            this.rpUser = rpUser;
            this.rpUserRoles = rpUserRoles;
            this.mapper = mapper;
            this.configration = configration;
            this.rpUserCustomerRef = rpUserCustomerRef;
        }
        public async Task<long> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
             var existUser = await rpUser.SingleOrDefaultAsync(x => x.Email == request.Email || x.UserName == request.UserName);
            if (existUser is not null)
                throw new ValidationException("Bu mail adresi veya kullanıcı adı başka bir kullanıcı tarafından zaten kullanılıyor!");
  
            var dbUser = mapper.Map<Domain.Entities.User, CreateUserCommandRequest>(request);
            dbUser.Password = PasswordEncryptor.Encrypt(request.Password);

            #region UploadProfileImage
            if (request.ProfileImage != null && request.ProfileImage.Length != 0)
            {
                var profileImagePath = configration.GetSection("UploadConfig").GetSection("UserProfileImagePath").Value;
                var profileImageUploadPath = Path.Combine(Directory.GetCurrentDirectory(), profileImagePath);
                UploadFileExtension.UploadImage(request.ProfileImage, profileImagePath, request.UserName + ".jpg", Width: 300, Height: 300, isFixed: false, x: 0, y: 0);
                dbUser.ProfileImage = profileImagePath + request.UserName + ".jpg";
            }
            #endregion

             
            var rows = await rpUser.AddAsync(dbUser);
            if (rows>0)
            {
                if (request.CustomerId != null && request.CustomerId.Count>0)
                {
                    foreach (var customer in request.CustomerId)
                    {
                        UserCustomerRef userCustomerRefItem = new();
                        userCustomerRefItem.UserId = dbUser.Id;
                        userCustomerRefItem.CustomerId = customer;
                        await rpUserCustomerRef.AddAsync(userCustomerRefItem);
                    }
                }
               

                foreach (var role in request.RoleId) 
                {
                    UserRoles roleItem = new();
                    roleItem.UserId = dbUser.Id;
                    roleItem.RoleId = role; 
                    await rpUserRoles.AddAsync(roleItem);
                }
            }
            return dbUser.Id;
        }

    }
}

