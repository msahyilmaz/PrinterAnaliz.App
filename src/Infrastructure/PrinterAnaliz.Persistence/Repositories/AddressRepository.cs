using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Application.Features.Addresses.Queries.GetAllAddress;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using PrinterAnaliz.Persistence.SqlContext;
using System.Security.Claims;

namespace PrinterAnaliz.Persistence.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private SqlDbContext addressRpContext;
        public AddressRepository(SqlDbContext _dbContext, ITokenService tokenService) : base(_dbContext)
        {
            var userAccessor = tokenService.GetUserAccessor();
            var transactionUser = userAccessor?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            _dbContext._transactionUser = transactionUser;
            addressRpContext = _dbContext;


        }

        public async Task<IList<GetAllAddressQueryResponseModel>> GetAllAddresses(string sp, SqlParameter[] sqlParameters)
        {
            return await addressRpContext.Database.SqlQueryRaw<GetAllAddressQueryResponseModel>(sp, sqlParameters)
                  .IgnoreQueryFilters()
                 .ToListAsync();
        }
    }
}
