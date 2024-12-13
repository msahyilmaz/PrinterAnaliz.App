using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Application.Features.Costumers.Command.Update;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.Costumers.Command.Delete
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommandRequest, long>
    {

        private readonly ICustomerRepository rpCustomer;

        public DeleteCustomerCommandHandler(ICustomerRepository rpCustomer)
        {
            this.rpCustomer = rpCustomer;
        }

        public async Task<long> Handle(DeleteCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            var qExistCustomer = await rpCustomer.Where(w => w.Id == request.Id && !w.IsDeleted).FirstOrDefaultAsync();
            if (qExistCustomer == null)
                throw new ValidationException("Müşteri bulunamadı.");


            qExistCustomer.IsDeleted = true;

            var updateCustomer = await rpCustomer.UpdateAsync(qExistCustomer);
            if (updateCustomer > 0)
                return qExistCustomer.Id;

            throw new Exception("Silme işlemi gerçekleşmedi, daha sonra tekrar deneyiniz.");
        }
    }
}
