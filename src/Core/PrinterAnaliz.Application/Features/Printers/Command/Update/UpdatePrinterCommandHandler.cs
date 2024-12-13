using FluentValidation;
using MediatR;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.Printers.Command.Update
{
    public class UpdatePrinterCommandHandler : IRequestHandler<UpdatePrinterCommandRequest, long>
    {
        private IPrinterRepository rpPrinter;
        private ICustomerRepository rpCustomer;
        private IGenericAutoMapper mapper;
        public UpdatePrinterCommandHandler(IPrinterRepository rpPrinter, ICustomerRepository rpCustomer, IGenericAutoMapper mapper)
        {
            this.rpPrinter = rpPrinter;
            this.rpCustomer = rpCustomer;
            this.mapper = mapper;

        }
        public async Task<long> Handle(UpdatePrinterCommandRequest request, CancellationToken cancellationToken)
        {
            var qCustomer = rpCustomer.Where(w => !w.IsDeleted && w.Id == request.CustomerId).FirstOrDefault();
            if (qCustomer == null)
                throw new ValidationException("Müşteri bulunamadı.");

            var qExistPrinter = rpPrinter.Where(w => !w.IsDeleted && w.CustomerId == request.CustomerId && w.Id == request.Id).FirstOrDefault();
            if (qExistPrinter == null)
                throw new ValidationException("Yazıcı bulunamadı.");

            request.IsClientActive ??= qExistPrinter.IsClientActive;
            request.IsPrinterActive ??= qExistPrinter.IsPrinterActive;

            var printer = mapper.Map<Printer, UpdatePrinterCommandRequest>(request, qExistPrinter, null, false);
            var addPrinter = await rpPrinter.UpdateAsync(printer);

            if (addPrinter > 0)
                return printer.Id;

            throw new Exception("Yazıcı güncellenemedi.");
        }
    }
}
