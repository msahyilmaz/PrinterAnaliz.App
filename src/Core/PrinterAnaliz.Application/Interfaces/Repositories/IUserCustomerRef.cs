using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Features.Users.Queries.GetAllUserRoles;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Interfaces.Repositories
{
    public interface IUserCustomerRef : IGenericRepository<UserCustomerRef>
    {
     //   Task<IList<GetAllUserRolesQueryResponse>> GetAllUserRoles(string sp, SqlParameter[] sqlParameters);
    }
  
}
