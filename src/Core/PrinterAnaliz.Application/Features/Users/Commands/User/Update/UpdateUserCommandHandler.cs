using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using PrinterAnaliz.Application.Enums;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using System.Linq;
using System.Security.Claims;

namespace PrinterAnaliz.Application.Features.Users.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommandRequest, long>
    {


        private readonly IUserRepository rpUser;
        private readonly IUserRolesRepository rpUserRoles;
        private readonly IUserCustomerRef rpUserCustomerRef;
        private readonly IGenericAutoMapper mapper;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configration;

        public UpdateUserCommandHandler(IUserRepository rpUser, IUserRolesRepository rpUserRoles, IGenericAutoMapper mapper, ITokenService tokenService, IConfiguration configration, IUserCustomerRef rpUserCustomerRef)
        {
            this.rpUser = rpUser;
            this.rpUserRoles = rpUserRoles;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.configration = configration;
            this.rpUserCustomerRef = rpUserCustomerRef;
        }
        public async Task<long> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            //var userAccessor = tokenService.GetUserAccessor();
            //var updateFrom = userAccessor.FindFirst(ClaimTypes.NameIdentifier).Value ?? throw new ValidationException("İşlem yapan kullanıcı bilgilerine erişilemedi.");
            var existUser = await rpUser.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);
            if (existUser is null)
                throw new ValidationException("Güncellenecek kullanıcı bulunamadı.");

            if (request.UserName != null)
            {
                var checkUserName = await rpUser.FirstOrDefaultAsync(x => x.Id != request.Id && x.UserName == request.UserName && !x.IsDeleted);
                if (checkUserName != null)
                    throw new ValidationException("Bu kullanıcı adı başka bir kullanıcı tarafından zaten kullanılıyor!");
            }
            if (request.Email != null)
            {
                var checkUserName = await rpUser.FirstOrDefaultAsync(x => x.Id != request.Id && x.Email == request.Email && !x.IsDeleted);
                if (checkUserName != null)
                    throw new ValidationException("Bu mail adresi başka bir kullanıcı tarafından zaten kullanılıyor!");
            }
            if (request.OldPassword != null && request.Password != null && existUser.Password != PasswordEncryptor.Encrypt(request.OldPassword))
               // throw new ValidationException("Eski şifreniz doğru değil!->" + PasswordEncryptor.Decrypt(existUser.Password));
                throw new ValidationException("Eski şifreniz doğru değil!->" + PasswordEncryptor.Decrypt(existUser.Password));

             

          //  await rpUserRoles.DeleteRangeAsync(d => d.UserId == request.Id);

            var dbUser = mapper.Map<Domain.Entities.User, UpdateUserCommandRequest>(request, existUser!, null, false);

            #region CheckingPassword
            if (request.Password != null && request.OldPassword != null)
                dbUser.Password = PasswordEncryptor.Encrypt(request.Password);
            else
                dbUser.Password = existUser.Password;
            #endregion

            #region UploadProfileImage
            if (request.ProfileImage != null && request.ProfileImage.Length != 0)
            {
                var profileImagePath = configration.GetSection("UploadConfig").GetSection("UserProfileImagePath").Value;
                var profileImageUploadPath = Path.Combine(Directory.GetCurrentDirectory(), profileImagePath);
                UploadFileExtension.UploadImage(request.ProfileImage, profileImagePath, existUser.UserName + ".jpg", Width: 300, Height: 300, isFixed: false, x: 0, y: 0);
                dbUser.ProfileImage = profileImagePath + existUser.UserName + ".jpg";
            }
            #endregion
             
            var rows = await rpUser.UpdateAsync(dbUser);

            #region CustomerControl
                if (request.CustomerId != null && request.CustomerId.Count > 0)
                {
                    await rpUserCustomerRef.SoftDeleteRangeAsync(x => x.UserId == dbUser.Id && !request.CustomerId.Contains(x.CustomerId));
                    var userCustomerRefList = request.CustomerId.Except(rpUserCustomerRef.Where(w => !w.IsDeleted && w.UserId == dbUser.Id).Select(s => s.CustomerId)).ToList();
                    if (userCustomerRefList != null)
                    {
                        foreach (var customer in userCustomerRefList)
                        {
                            UserCustomerRef userCustomerRefItem = new();
                            userCustomerRefItem.UserId = dbUser.Id;
                            userCustomerRefItem.CustomerId = customer;
                            await rpUserCustomerRef.AddAsync(userCustomerRefItem);
                        }

                    }

                }
            #endregion
            #region RolControl
                if (request.RoleId != null && request.RoleId.Count > 0)
                {

                    List<int> convertUserRolesList = new List<int>();
                    foreach (var role in request.RoleId)
                    {
                        if (Enum.IsDefined(typeof(UserRoleTypes), role))
                            convertUserRolesList.Add(role);
                    }
                    if (convertUserRolesList != null && convertUserRolesList.Count > 0)
                    {
                        await rpUserRoles.SoftDeleteRangeAsync(x => x.UserId == dbUser.Id && !convertUserRolesList.Contains(x.RoleId));
                        var userRolesList = convertUserRolesList.Except(rpUserRoles.Where(w => !w.IsDeleted && w.UserId == dbUser.Id).Select(s => s.RoleId)).ToList();
                        foreach (var role in userRolesList)
                        {
                            UserRoles roleItem = new();
                            roleItem.UserId = dbUser.Id;
                            roleItem.RoleId = role;
                            await rpUserRoles.AddAsync(roleItem);
                        }
                    }
                   
                }
            #endregion


            return dbUser.Id;
        }
    }
}
