using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PrinterAnaliz.Application.Enums;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.PrinterLogs.Command.Create
{
    public class CreatePrinterLogCommandHandler : IRequestHandler<CreatePrinterLogCommandRequest, long>
    {
        private readonly IPrinterLogRepository rpPrinterLog;
        private readonly IPrinterRepository rpPrinter;
        private readonly ITokenService tokenService;
        private readonly IGenericAutoMapper mapper;

        public CreatePrinterLogCommandHandler(IPrinterLogRepository rpPrinterLog, IPrinterRepository rpPrinter, ITokenService tokenService, IGenericAutoMapper mapper)
        {
            this.rpPrinterLog = rpPrinterLog;
            this.rpPrinter = rpPrinter;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }
        public async Task<long> Handle(CreatePrinterLogCommandRequest request, CancellationToken cancellationToken)
        {
            var userAccessor = tokenService.GetLoginedUser();

            if (userAccessor.UserRoles.Any(x => x == UserRoleTypes.Viewer /*|| x != UserRoleTypes.Admin*/))
            {
                var getCustomerPrinters = rpPrinter.Where(w=>!w.IsDeleted && userAccessor.Customers.Contains(w.CustomerId) && w.Id == request.PrinterId).ToList();
                if (getCustomerPrinters == null || getCustomerPrinters.Count()==0)
                    throw new Exception("Log eklenecek yazıcı bulunamadı.");
            }
            var map = mapper.Map<PrinterLog, CreatePrinterLogCommandRequest>(request);
            var result =await rpPrinterLog.AddAsync(map);

            return map.Id;
        }
    }
}
