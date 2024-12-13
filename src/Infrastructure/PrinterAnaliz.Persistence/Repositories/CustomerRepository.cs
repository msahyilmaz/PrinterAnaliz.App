using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Application.Features.Costumers.Queries.GetAllCustomer;
using PrinterAnaliz.Application.Features.Users.Queries.GetAllUsers;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using PrinterAnaliz.Persistence.SqlContext;
using System.Security.Claims;

namespace PrinterAnaliz.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private SqlDbContext customerRpContext;
        public CustomerRepository(SqlDbContext _dbContext, ITokenService tokenService) : base(_dbContext)
        {
            customerRpContext = _dbContext;
            var userAccessor = tokenService.GetUserAccessor();
            var transactionUser = userAccessor?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            _dbContext._transactionUser = transactionUser;
        }
        public async Task<IList<GetAllCustomerQueryResponseModel>> GetAllCustomer(string sp, SqlParameter[] sqlParameters)
        {
            return await customerRpContext.Database.SqlQueryRaw<GetAllCustomerQueryResponseModel>(sp, sqlParameters)
                  .IgnoreQueryFilters()
                 .ToListAsync();
        }
    }
}
