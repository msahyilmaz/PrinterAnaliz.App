using MediatR;
using Newtonsoft.Json.Linq;
using PrinterAnaliz.Application.Extensions;

namespace PrinterAnaliz.Application.Features.Costumers.Queries.GetAllCustomer
{
    public class GetAllCustomerQueryRequest : GenericQueryModel, IRequest<GenericResponseModel<IList<GetAllCustomerQueryResponse>>>
    {
        private string customerId;
        public string CustomerId { get { return customerId; }  set { customerId = value.ConvertSPIdList(); } }
    }
}
