using FluentValidation;
using MediatR;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;

namespace PrinterAnaliz.Application.Features.Printers.Command.Delete
{
    public class DeletePrinterCommandHandler : IRequestHandler<DeletePrinterCommandRequest, long>
    {
        private IPrinterRepository rpPrinter;
        private ICustomerRepository rpCustomer; 
        public DeletePrinterCommandHandler(IPrinterRepository rpPrinter, ICustomerRepository rpCustomer )
        {
            this.rpPrinter = rpPrinter;
            this.rpCustomer = rpCustomer; 

        }
        public async Task<long> Handle(DeletePrinterCommandRequest request, CancellationToken cancellationToken)
        {
            var qCustomer = rpCustomer.Where(w => !w.IsDeleted && w.Id == request.CustomerId).FirstOrDefault();
            if (qCustomer == null)
                throw new ValidationException("Müşteri bulunamadı.");

            var qExistPrinter = rpPrinter.Where(w => !w.IsDeleted && w.CustomerId == request.CustomerId && w.Id == request.Id).FirstOrDefault();
            if (qExistPrinter == null)
                throw new ValidationException("Yazıcı bulunamadı.");

             qExistPrinter.IsDeleted = true;

             var addPrinter = await rpPrinter.UpdateAsync(qExistPrinter);

            if (addPrinter > 0)
                return qExistPrinter.Id;

            throw new Exception("Yazıcı silinemedi.");
        }
    }
}
