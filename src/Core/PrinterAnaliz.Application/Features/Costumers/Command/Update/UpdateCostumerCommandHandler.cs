using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Application.Features.Customers.Command.Create;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.Costumers.Command.Update
{
    public class UpdateCostumerCommandHandler : IRequestHandler<UpdateCostumerCommandRequest, long>
    {
        private readonly ICustomerRepository rpCustomer;
        private readonly IGenericAutoMapper mapper;

        public UpdateCostumerCommandHandler(ICustomerRepository rpCustomer, IGenericAutoMapper mapper)
        {
            this.rpCustomer = rpCustomer;
            this.mapper = mapper;
        }
        public async Task<long> Handle(UpdateCostumerCommandRequest request, CancellationToken cancellationToken)
        {
            var qExistCustomer = await rpCustomer.Where(w => w.Id == request.Id && !w.IsDeleted).FirstOrDefaultAsync();
            if (qExistCustomer == null)
                throw new ValidationException("Müşteri bulunamadı.");

            var qCustomer = await rpCustomer.Where(w => w.Name == request.Name && w.Id != request.Id && !w.IsDeleted).FirstOrDefaultAsync();
            if (qCustomer != null)
                throw new ValidationException("Bu isimde bir müşteri tanımlı.");
           

            var map = mapper.Map<Customer, UpdateCostumerCommandRequest>(request,qExistCustomer,null,false);
            var updateCustomer = await rpCustomer.UpdateAsync(map);
            if (updateCustomer > 0)
                return map.Id;

            throw new Exception("Güncelleme işlemi gerçekleşmedi, daha sonra tekrar deneyiniz.");

        }
    }
}
