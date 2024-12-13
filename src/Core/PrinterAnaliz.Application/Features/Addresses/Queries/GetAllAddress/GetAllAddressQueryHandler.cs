using MediatR;
using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Enums;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;

namespace PrinterAnaliz.Application.Features.Addresses.Queries.GetAllAddress
{
    public class GetAllAddressQueryHandler:IRequestHandler<GetAllAddressQueryRequest, GenericResponseModel<IList<GetAllAddressQueryResponse>>>
    {

        private readonly IAddressRepository rpAddress;
        private readonly ITokenService tokenService;
        private readonly IGenericAutoMapper mapper;
        public GetAllAddressQueryHandler(IAddressRepository rpAddress, ITokenService tokenService,IGenericAutoMapper mapper)
        {
            this.rpAddress = rpAddress;
            this.tokenService = tokenService;
            this.mapper=mapper;
        }

        public async Task<GenericResponseModel<IList<GetAllAddressQueryResponse>>> Handle(GetAllAddressQueryRequest request, CancellationToken cancellationToken)
        {
            var result = new GenericResponseModel<IList<GetAllAddressQueryResponse>>();
            var loginedUser = tokenService.GetLoginedUser();
            if (loginedUser.UserRoles.Any(x => x == UserRoleTypes.Viewer /*|| x != UserRoleTypes.Admin*/))
            {
                request.UserId = loginedUser.Id.ToString();
            }

            SqlParameter[] sp_User_getAllParameters = {                    new SqlParameter("@Search", String.IsNullOrEmpty(request.Search) ? (object)DBNull.Value : request.Search),                    new SqlParameter("@UserId", String.IsNullOrEmpty(request.UserId) ? (object)DBNull.Value : request.UserId),                    new SqlParameter("@CustomerId", String.IsNullOrEmpty(request.CustomerId) ? (object)DBNull.Value : request.CustomerId),                    new SqlParameter("@AddressType",String.IsNullOrEmpty(request.AddressType) || !request.AddressType.All(char.IsDigit) ? (object)DBNull.Value : request.AddressType),                    new SqlParameter("@pageIndex", request.PageIndex),                    new SqlParameter("@ItemCount", request.ItemCount),                    new SqlParameter("@ItemOrderBy",String.IsNullOrEmpty(request.OrderBy) ? "ID DESC" :request.OrderBy)                };
            var addresses = await rpAddress.GetAllAddresses("EXECUTE Sp_Address_getAll @Search,@UserId,@CustomerId,@AddressType, @pageIndex, @ItemCount, @ItemOrderBy", sp_User_getAllParameters);
            var map = mapper.Map<GetAllAddressQueryResponse, GetAllAddressQueryResponseModel>(addresses);
            result.Data = map;
            result.Succeeded = true;
            result.TotalRecordCount = addresses.FirstOrDefault()?.TotalRecordCount ?? 0;
            result.PageIndex = request.PageIndex;
            result.ItemCount = request.ItemCount;
            result.OrderBy = request.OrderBy;

            return result;
        }
    }
}
