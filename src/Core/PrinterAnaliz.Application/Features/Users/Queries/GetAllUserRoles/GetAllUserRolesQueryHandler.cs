using MediatR;
using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Interfaces.Repositories;

namespace PrinterAnaliz.Application.Features.Users.Queries.GetAllUserRoles
{
    public class GetAllUserRolesQueryHandler:IRequestHandler<GetAllUserRolesQueryRequest, IList<GetAllUserRolesQueryResponse>>
    {
      
        private readonly IUserRolesRepository rpUserRoles;

        public GetAllUserRolesQueryHandler(IUserRepository rpUser, IUserRolesRepository rpUserRoles)
        { 
            this.rpUserRoles = rpUserRoles; 
        }

        public async Task<IList<GetAllUserRolesQueryResponse>> Handle(GetAllUserRolesQueryRequest request, CancellationToken cancellationToken)
        {
           
            SqlParameter[] sp_UserRoles_getAllParameters = {                     new SqlParameter("@UserId", String.IsNullOrEmpty(request.UserId) ? (object)DBNull.Value : request.UserId)
            };
             
            return await rpUserRoles.GetAllUserRoles("EXECUTE Sp_UserRoles_getAll @UserId", sp_UserRoles_getAllParameters);

        }
    }
}
