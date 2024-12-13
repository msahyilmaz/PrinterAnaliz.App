using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.Customers.Command.Create
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommandRequest, long>
    {
        private readonly ICustomerRepository rpCustomer;
        private readonly IGenericAutoMapper mapper;

        public CreateCustomerCommandHandler(ICustomerRepository rpCustomer,IGenericAutoMapper mapper)
        {
            this.rpCustomer = rpCustomer;
            this.mapper = mapper;
        }
    

        public async Task<long> Handle(CreateCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            var qCustomer = await rpCustomer.Where(w => w.Name == request.Name && !w.IsDeleted).FirstOrDefaultAsync();
            if (qCustomer != null)
                throw new ValidationException("Bu isimde bir müşteri tanımlı.");
            var map = mapper.Map<Customer, CreateCustomerCommandRequest>(request);
            var addCustom = await rpCustomer.AddAsync(map);
            if (addCustom > 0)
                return map.Id;
            
            throw new Exception("Kayıt işlemi gerçekleşmedi, daha sonra tekrar deneyiniz.");

        }
    }
}
