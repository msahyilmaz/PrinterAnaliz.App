using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Application.Features.Users.Queries.GetAllUserRoles;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using PrinterAnaliz.Persistence.SqlContext;
using System.Security.Claims;

namespace PrinterAnaliz.Persistence.Repositories
{
    public class UserRolesRepository : GenericRepository<UserRoles>, IUserRolesRepository
    {
        private SqlDbContext userRoleRpContext;
        public UserRolesRepository(SqlDbContext _dbContext, ITokenService tokenService) : base(_dbContext)
        {
            var userAccessor = tokenService.GetUserAccessor();
            var transactionUser = userAccessor?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            _dbContext._transactionUser = transactionUser;
            userRoleRpContext = _dbContext;
        }
        public async Task<IList<GetAllUserRolesQueryResponse>> GetAllUserRoles(string sp, SqlParameter[] sqlParameters)
        {
            return await userRoleRpContext.Database.SqlQueryRaw<GetAllUserRolesQueryResponse>(sp, sqlParameters)
                  .IgnoreQueryFilters()
                 .ToListAsync();
        }
    }
}
