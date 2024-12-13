using MediatR;
using PrinterAnaliz.Application.Extensions;

namespace PrinterAnaliz.Application.Features.Users.Queries.GetAllUserRoles
{
    public class GetAllUserRolesQueryRequest : IRequest<IList<GetAllUserRolesQueryResponse>>
    {
        private string userId;
        public string UserId
        {
            get {  return userId.ConvertSPIdList();  }
            set { userId = value;  }
        }
    }
}
