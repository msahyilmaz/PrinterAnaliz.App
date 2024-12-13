using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Features.Addresses.Queries.GetAllAddress;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Interfaces.Repositories
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<IList<GetAllAddressQueryResponseModel>> GetAllAddresses(string sp, SqlParameter[] sqlParameters);
    }
}
