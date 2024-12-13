using MediatR;
using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Enums;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;

namespace PrinterAnaliz.Application.Features.PrinterLogs.Queries.GetAllPrinterLogs
{
    public class GetAllPrinterLogsQueryHandler : IRequestHandler<GetAllPrinterLogsQueryRequest, GenericResponseModel<IList<GetAllPrinterLogsQueryResponse>>>
    {
        private readonly IPrinterLogRepository rpPrinterLog;
        private readonly ITokenService tokenService;
        private readonly IUserCustomerRef rpUserCustomerRef;
        private readonly IGenericAutoMapper mapper;
        public GetAllPrinterLogsQueryHandler(IPrinterLogRepository rpPrinterLog, ITokenService tokenService, IUserCustomerRef rpUserCustomerRef, IGenericAutoMapper mapper)
        {
            this.rpPrinterLog = rpPrinterLog;
            this.tokenService = tokenService;
            this.rpUserCustomerRef = rpUserCustomerRef;
            this.mapper = mapper;
        }
        public async Task<GenericResponseModel<IList<GetAllPrinterLogsQueryResponse>>> Handle(GetAllPrinterLogsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = new GenericResponseModel<IList<GetAllPrinterLogsQueryResponse>>();
            var loginedUser = tokenService.GetLoginedUser();

            #region CustomersFilter
            if (loginedUser.UserRoles.Any(x => x == UserRoleTypes.Viewer /*|| x != UserRoleTypes.Admin*/))
            {
                request.StrCustomerId = string.Join(",", loginedUser.Customers.ToArray());
                if (!string.IsNullOrEmpty(request.StrCustomerId))
                {
                    var requestCustumers = request.StrCustomerId.Split(',')?.Where(x => int.TryParse(x, out _)).Select(Int64.Parse)?.ToList();
                    if (requestCustumers?.Count() > 0)
                    {
                        List<long> newCustomerId = new List<long>();
                        foreach (var customerId in loginedUser.Customers)
                        {
                            if (requestCustumers.Contains(customerId))
                                newCustomerId.Add(customerId);
                        }
                        if (newCustomerId.Count() > 0)
                            request.StrCustomerId = String.Join(", ", newCustomerId.ToArray());
                    }
                }
            }
            #endregion

            SqlParameter[] sp_PrinterLog_getAllParameters = {                    new SqlParameter("@PrinterId", String.IsNullOrEmpty(request.PrinterId) ? (object)DBNull.Value : request.PrinterId),                    new SqlParameter("@StrCustomerId", String.IsNullOrEmpty(request.StrCustomerId) ? (object)DBNull.Value : request.StrCustomerId),                    new SqlParameter("@StartDate", request.StartDate == null || request.StartDate == DateTime.MinValue ? (object)DBNull.Value : request.StartDate),                    new SqlParameter("@EndDate", request.EndDate == null || request.EndDate == DateTime.MinValue ? (object)DBNull.Value : request.EndDate),                    new SqlParameter("@pageIndex", request.PageIndex),                    new SqlParameter("@ItemCount", request.ItemCount),                    new SqlParameter("@ItemOrderBy",String.IsNullOrEmpty(request.OrderBy) ? "ID DESC" :request.OrderBy),                };
            var qPrinterLogs = await rpPrinterLog.GetAllPrinterLogs("EXECUTE Sp_PrinterLog_getAll @PrinterId, @StrCustomerId, @StartDate,  @EndDate, @PageIndex, @ItemCount,@ItemOrderBy", sp_PrinterLog_getAllParameters);
            var map = mapper.Map<GetAllPrinterLogsQueryResponse, GetAllPrinterLogsQueryResponseModel>(qPrinterLogs);
            result.Data = map;
            result.Succeeded = true;
            result.TotalRecordCount = qPrinterLogs?.FirstOrDefault()?.TotalRecordCount ?? 0;
            result.PageIndex = request.PageIndex;
            result.ItemCount = request.ItemCount;
            result.OrderBy = request.OrderBy;

            return result;
        }
    }
}
