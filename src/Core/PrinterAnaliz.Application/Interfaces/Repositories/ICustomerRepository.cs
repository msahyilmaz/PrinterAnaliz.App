using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Features.Costumers.Queries.GetAllCustomer;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Interfaces.Repositories
{
    public interface ICustomerRepository:IGenericRepository<Customer>
    {
        Task<IList<GetAllCustomerQueryResponseModel>> GetAllCustomer(string sp, SqlParameter[] sqlParameters);
    }

}
