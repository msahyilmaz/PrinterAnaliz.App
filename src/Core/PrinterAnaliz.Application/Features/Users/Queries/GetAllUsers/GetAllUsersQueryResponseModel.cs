using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryResponseModel: GetAllUsersQueryResponse
    { 
        public int TotalRecordCount { get; set; } 

    }
}
