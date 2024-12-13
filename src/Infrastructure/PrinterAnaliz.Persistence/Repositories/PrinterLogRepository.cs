using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Application.Features.PrinterLogs.Queries.GetAllPrinterLogs;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using PrinterAnaliz.Persistence.SqlContext;
using System.Security.Claims;

namespace PrinterAnaliz.Persistence.Repositories
{
    public class PrinterLogRepository : GenericRepository<PrinterLog>, IPrinterLogRepository
    {
        private SqlDbContext printerLogRpContext;
        public PrinterLogRepository(SqlDbContext _dbContext, ITokenService tokenService) : base(_dbContext)
        {
            var userAccessor = tokenService.GetUserAccessor();
            var transactionUser = userAccessor?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            _dbContext._transactionUser = transactionUser;
            printerLogRpContext = _dbContext;
        }
        public async Task<IList<GetAllPrinterLogsQueryResponseModel>> GetAllPrinterLogs(string sp, SqlParameter[] sqlParameters)
        {
            return await printerLogRpContext.Database.SqlQueryRaw<GetAllPrinterLogsQueryResponseModel>(sp, sqlParameters)
                  .IgnoreQueryFilters()
                 .ToListAsync();
        }
    }
}
