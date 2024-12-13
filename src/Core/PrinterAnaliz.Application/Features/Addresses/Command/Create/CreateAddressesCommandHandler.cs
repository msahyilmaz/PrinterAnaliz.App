using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Application.Enums;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.Addresses.Command.Create
{
    public class CreateAddressesCommandHandler : IRequestHandler<CreateAddressesCommandRequest, long>
    {
        private readonly IAddressRepository rpAddress;
        private readonly IUserRepository rpUser;
        private readonly IGenericAutoMapper mapper;
        private readonly ITokenService tokenService;

        public CreateAddressesCommandHandler(IAddressRepository rpAddress, IUserRepository rpUser, IGenericAutoMapper mapper, ITokenService tokenService)
        {
            this.rpAddress = rpAddress;
            this.rpUser = rpUser;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }
        public async Task<long> Handle(CreateAddressesCommandRequest request, CancellationToken cancellationToken)
        {
            var userAccessor = tokenService.GetLoginedUser();
            if (userAccessor.UserRoles.Any(x => x == UserRoleTypes.Viewer /*|| x != UserRoleTypes.Admin*/))
            {
                request.UserId = userAccessor.Id;
            }

            var qUser = rpUser.Where(w=>w.Id == request.UserId && !w.IsDeleted).FirstOrDefault();
            if (qUser == null)
                throw new ValidationException("Kullanıcı bulunamadı.");

            var qAddress = await rpAddress.Where(w=>w.UserId == request.UserId && !w.IsDeleted).ToListAsync();

            if (qAddress != null && qAddress.Count()>4)
                throw new ValidationException("Bir kullanıcı maksimum 4 (dört) adet adres ekleyebilir.");
            
            


            var dbAddress = mapper.Map<Address, CreateAddressesCommandRequest>(request);
            var addAddress = await rpAddress.AddAsync(dbAddress);
            if (addAddress > 0)
                return dbAddress.Id;
            else
                throw new Exception("Addres kaydedilemedi.");
        }
    }
}
