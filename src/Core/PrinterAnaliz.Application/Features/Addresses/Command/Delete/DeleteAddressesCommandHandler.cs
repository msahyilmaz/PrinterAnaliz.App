using FluentValidation;
using MediatR;
using PrinterAnaliz.Application.Enums;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;

namespace PrinterAnaliz.Application.Features.Addresses.Command.Delete
{
    public class DeleteAddressesCommandHandler : IRequestHandler<DeleteAddressesCommandRequest, long>
    {


        private readonly IAddressRepository rpAddress;
        private readonly IUserRepository rpUser;
        private readonly IGenericAutoMapper mapper;
        private readonly ITokenService tokenService;



        public DeleteAddressesCommandHandler(IAddressRepository rpAddress, IUserRepository rpUser, IGenericAutoMapper mapper, ITokenService tokenService)
        {
            this.rpAddress = rpAddress;
            this.rpUser = rpUser;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        public async Task<long> Handle(DeleteAddressesCommandRequest request, CancellationToken cancellationToken)
        {
            var userAccessor = tokenService.GetLoginedUser();

            var qAddress = rpAddress.Where(w => w.Id == request.Id && !w.IsDeleted);

            if (userAccessor.UserRoles.Any(x => x == UserRoleTypes.Viewer /*|| x != UserRoleTypes.Admin*/))
                qAddress = rpAddress.Where(w => w.Id == request.Id && w.UserId == userAccessor.Id && !w.IsDeleted);

            var resultAddress = qAddress.FirstOrDefault();

            if (resultAddress == null)
                throw new ValidationException("Adres bulunamadı.");

            resultAddress.IsDeleted = true;

            if (await rpAddress.UpdateAsync(resultAddress) > 0)
                return request.Id;
            else
                throw new Exception("Adres güncellenemedi.");

        }
    }
}
