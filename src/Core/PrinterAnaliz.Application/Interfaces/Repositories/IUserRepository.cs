using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Features.Users.Queries.GetAllUsers;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Interfaces.Repositories
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<IList<GetAllUsersQueryResponseModel>> GetAllUsers(string sp, SqlParameter[] sqlParameters);
    }
}
