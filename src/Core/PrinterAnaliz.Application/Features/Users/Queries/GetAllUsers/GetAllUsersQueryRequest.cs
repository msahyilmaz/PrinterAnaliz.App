using MediatR;
using PrinterAnaliz.Application.Extensions;

namespace PrinterAnaliz.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryRequest : GenericQueryModel, IRequest<GenericResponseModel<IList<GetAllUsersQueryResponse>>>
    {
        private string userId;
        public string UserId
        {
            get { return userId.ConvertSPIdList(); }
            set { userId = value; }
        }
        public int RolId { get; set; }
        public string EmailNotification { get; set; }
        public string CanOrder { get; set; }
    }
}
