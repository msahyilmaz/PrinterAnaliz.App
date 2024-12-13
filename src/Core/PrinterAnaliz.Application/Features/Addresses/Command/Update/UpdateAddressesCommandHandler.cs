using FluentValidation;
using MediatR;
using PrinterAnaliz.Application.Enums;
using PrinterAnaliz.Application.Features.Addresses.Command.Create;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using System;

namespace PrinterAnaliz.Application.Features.Addresses.Command.Update
{
    public class UpdateAddressesCommandHandler : IRequestHandler<UpdateAddressesCommandRequest, long>
    {
        private readonly IAddressRepository rpAddress;
        private readonly IUserRepository rpUser;
        private readonly IGenericAutoMapper mapper;
        private readonly ITokenService tokenService;

        

        public UpdateAddressesCommandHandler(IAddressRepository rpAddress, IUserRepository rpUser, IGenericAutoMapper mapper, ITokenService tokenService)
        {
            this.rpAddress = rpAddress;
            this.rpUser = rpUser;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }
        public async Task<long> Handle(UpdateAddressesCommandRequest request, CancellationToken cancellationToken)
        {
            var userAccessor = tokenService.GetLoginedUser();

            if (userAccessor.UserRoles.Any(x => x == UserRoleTypes.Viewer /*|| x != UserRoleTypes.Admin*/)) 
            {
                request.UserId = userAccessor.Id;
            }

            var qAddress = rpAddress.Where(w => w.Id == request.Id && w.UserId == request.UserId && !w.IsDeleted).FirstOrDefault();
            if (qAddress == null)
                throw new ValidationException("Adres bulunamadı.");

            var qUser = rpUser.Where(w => w.Id == request.UserId && !w.IsDeleted).FirstOrDefault();
            if (qUser == null)
                throw new ValidationException("Kullanıcı bulunamadı.");

            var dbAddress = mapper.Map<Address, UpdateAddressesCommandRequest>(request, qAddress!, null, false);
            var addAddress = await rpAddress.UpdateAsync(dbAddress);
            if (addAddress > 0)
                return dbAddress.Id;
            else
                throw new Exception("Addres güncellenemedi.");

        }
    }
}
