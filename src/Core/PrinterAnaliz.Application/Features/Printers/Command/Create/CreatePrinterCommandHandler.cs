using FluentValidation;
using MediatR;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.Printers.Command.Create
{
    public class CreatePrinterCommandHandler : IRequestHandler<CreatePrinterCommandRequest, long>
    {
        private IPrinterRepository rpPrinter;
        private ICustomerRepository rpCustomer;
        private IGenericAutoMapper mapper;
        public CreatePrinterCommandHandler(IPrinterRepository rpPrinter, ICustomerRepository rpCustomer,IGenericAutoMapper mapper)
        {
            this.rpPrinter = rpPrinter;
            this.rpCustomer = rpCustomer;
            this.mapper = mapper;

        }
        public async Task<long> Handle(CreatePrinterCommandRequest request, CancellationToken cancellationToken)
        {
            var qCustomer = rpCustomer.Where(w => !w.IsDeleted && w.Id == request.CustomerId).FirstOrDefault();
            if (qCustomer == null)
                throw new ValidationException("Müşteri bulunamadı.");

            var printer = mapper.Map<Printer, CreatePrinterCommandRequest>(request);
            var addPrinter = await rpPrinter.AddAsync(printer);

            if (addPrinter > 0)
                return printer.Id;

            throw new Exception("Yazıcı ekelenemedi.");
        }
    }
}
