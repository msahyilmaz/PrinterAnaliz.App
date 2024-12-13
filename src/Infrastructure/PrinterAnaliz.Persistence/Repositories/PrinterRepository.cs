using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Application.Features.Printers.Queries.GetAllPrinter;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Application.Interfaces.Tokens;
using PrinterAnaliz.Domain.Entities;
using PrinterAnaliz.Persistence.SqlContext;
using System.Security.Claims;

namespace PrinterAnaliz.Persistence.Repositories
{
    public class PrinterRepository : GenericRepository<Printer>, IPrinterRepository
    {
        private SqlDbContext printerRpContext;
        public PrinterRepository(SqlDbContext _dbContext, ITokenService tokenService) : base(_dbContext)
        {
            var userAccessor = tokenService.GetUserAccessor();
            var transactionUser = userAccessor?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            _dbContext._transactionUser = transactionUser;
            printerRpContext = _dbContext;
        }
        public async Task<IList<GetAllPrinterResponseModel>> GetAllPrinters(string sp, SqlParameter[] sqlParameters)
        {
            return await printerRpContext.Database.SqlQueryRaw<GetAllPrinterResponseModel>(sp, sqlParameters)
                  .IgnoreQueryFilters()
                 .ToListAsync();
        }
    }
}
