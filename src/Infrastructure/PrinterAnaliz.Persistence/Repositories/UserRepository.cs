using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Application.Features.Users.Queries.GetAllUsers;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using PrinterAnaliz.Persistence.SqlContext;
using System.Security.Claims;

namespace PrinterAnaliz.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
         
        private SqlDbContext userRpContext;
        public UserRepository(SqlDbContext _dbContext, ITokenService tokenService) : base(_dbContext)
        {
            var userAccessor = tokenService.GetUserAccessor();
            var transactionUser = userAccessor?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            _dbContext._transactionUser = transactionUser;
            userRpContext = _dbContext;
        }

        public async Task<IList<GetAllUsersQueryResponseModel>> GetAllUsers(string sp, SqlParameter[] sqlParameters) 
        {
            
          
            return await userRpContext.Database.SqlQueryRaw<GetAllUsersQueryResponseModel>(sp, sqlParameters) 
                  .IgnoreQueryFilters()
                 .ToListAsync();
        }
    }
}
