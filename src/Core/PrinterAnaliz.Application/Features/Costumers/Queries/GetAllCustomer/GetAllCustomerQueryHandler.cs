using MediatR;
using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;

namespace PrinterAnaliz.Application.Features.Costumers.Queries.GetAllCustomer
{
    public class GetAllCustomerQueryHandler : IRequestHandler<GetAllCustomerQueryRequest, GenericResponseModel<IList<GetAllCustomerQueryResponse>>>
    {
        private readonly ICustomerRepository rpCustomer;
        private readonly IGenericAutoMapper mapper;

        public GetAllCustomerQueryHandler(ICustomerRepository rpCustomer, IGenericAutoMapper mapper)
        {
            this.rpCustomer = rpCustomer;
            this.mapper = mapper;
        }
        public async Task<GenericResponseModel<IList<GetAllCustomerQueryResponse>>> Handle(GetAllCustomerQueryRequest request, CancellationToken cancellationToken)
        {
            var result = new GenericResponseModel<IList<GetAllCustomerQueryResponse>>();
            SqlParameter[] sp_Customer_getAllParameters = {
                    new SqlParameter("@Search", String.IsNullOrEmpty(request.Search) ? (object)DBNull.Value : request.Search),
                    new SqlParameter("@CustomerId", String.IsNullOrEmpty(request.CustomerId) ? (object)DBNull.Value : request.CustomerId),
                    new SqlParameter("@PageIndex", request.PageIndex),
                    new SqlParameter("@ItemCount", request.ItemCount),
                    new SqlParameter("@ItemOrderBy",String.IsNullOrEmpty(request.OrderBy) ? "ID DESC" :request.OrderBy),
            };

            var customers = await rpCustomer.GetAllCustomer("EXECUTE Sp_Customer_getAll @Search,@CustomerId,@PageIndex,@ItemCount,@ItemOrderBy", sp_Customer_getAllParameters);
            
            var map = mapper.Map<GetAllCustomerQueryResponse, GetAllCustomerQueryResponseModel>(customers);
            result.Data = map;
            result.Succeeded = true;
            result.TotalRecordCount = customers.FirstOrDefault()?.TotalRecordCount ?? 0;
            result.PageIndex = request.PageIndex;
            result.ItemCount = request.ItemCount;
            result.OrderBy = request.OrderBy;
            return result;
        }
    }
}
