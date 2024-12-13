using MediatR;
using PrinterAnaliz.Application.Enums;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.Addresses.Queries.GetAllAddress
{
    public class GetAllAddressQueryRequest: GenericQueryModel, IRequest<GenericResponseModel<IList<GetAllAddressQueryResponse>>>
    {
        private string userId;
        private string customerId;
        private string addressType;
        public string UserId { get { return userId; } set { userId = value.ConvertSPIdList(); } }
        public string CustomerId { get { return customerId; } set { customerId = value.ConvertSPIdList(); } }
        public string AddressType { get { return addressType; } set { addressType = value.ConvertSPIdList(); } }
    }
}
