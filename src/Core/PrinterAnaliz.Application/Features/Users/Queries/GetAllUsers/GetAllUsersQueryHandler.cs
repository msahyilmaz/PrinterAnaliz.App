using MediatR;
using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;

namespace PrinterAnaliz.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, GenericResponseModel<IList<GetAllUsersQueryResponse>>>
    {
        private readonly IUserRepository rpUser;
        private readonly IUserRolesRepository rpUserRoles;
        private readonly IGenericAutoMapper mapper;

        public GetAllUsersQueryHandler(IUserRepository rpUser, IUserRolesRepository rpUserRoles, IGenericAutoMapper mapper)
        {
            this.rpUser = rpUser;
            this.rpUserRoles = rpUserRoles;
            this.mapper = mapper;
        }
        public async Task<GenericResponseModel<IList<GetAllUsersQueryResponse>>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var result = new GenericResponseModel<IList<GetAllUsersQueryResponse>>();
            var emailNotification = request.EmailNotification switch{"0" => 0,"1" => 1,_ => (object)DBNull.Value};
            var canOrder = request.CanOrder switch {"0" => 0,"1" => 1,_ => (object)DBNull.Value};

            SqlParameter[] sp_User_getAllParameters = {                    new SqlParameter("@Search", String.IsNullOrEmpty(request.Search) ? (object)DBNull.Value : request.Search),                    new SqlParameter("@UserId", String.IsNullOrEmpty(request.UserId) ? (object)DBNull.Value : request.UserId),                    new SqlParameter("@EmailNotification", emailNotification),                    new SqlParameter("@CanOrder", canOrder),                    new SqlParameter("@pageIndex", request.PageIndex),                    new SqlParameter("@ItemCount", request.ItemCount),                    new SqlParameter("@ItemOrderBy",String.IsNullOrEmpty(request.OrderBy) ? "ID DESC" :request.OrderBy),                    new SqlParameter("@RolId", request.RolId == 0 ? (object)DBNull.Value : request.RolId)                };

            var users = await rpUser.GetAllUsers("EXECUTE Sp_User_getAll @Search,@UserId,@EmailNotification,@CanOrder, @pageIndex, @ItemCount, @ItemOrderBy, @RolId", sp_User_getAllParameters);

            var map = mapper.Map<GetAllUsersQueryResponse, GetAllUsersQueryResponseModel>(users);
            if (map.Count()>0)
            {
                var userIdString = String.Join(",", map.Select(x => x.Id.ToString()).ToArray());
                SqlParameter[] sp_UserRoles_getAllParameters = { new SqlParameter("@UserId", userIdString) };
                var userRoleList = await rpUserRoles.GetAllUserRoles("EXECUTE Sp_UserRoles_getAll @UserId", sp_UserRoles_getAllParameters);
                foreach (var mapItem in map) 
                {
                    mapItem.UserRoles = userRoleList.Where(w => w.UserId == mapItem.Id).ToList();
                }
            }

            result.Data =  map;
            result.Succeeded = true;
            result.TotalRecordCount = users.FirstOrDefault()?.TotalRecordCount ?? 0;
            result.PageIndex = request.PageIndex;
            result.ItemCount = request.ItemCount;
            result.OrderBy = request.OrderBy;  

            return result;
        }
    }
}
