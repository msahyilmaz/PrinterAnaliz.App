using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Features.PrinterLogs.Queries.GetAllPrinterLogs;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Interfaces.Repositories
{
    public interface IPrinterLogRepository : IGenericRepository<PrinterLog>
    {
        Task<IList<GetAllPrinterLogsQueryResponseModel>> GetAllPrinterLogs(string sp, SqlParameter[] sqlParameters);
    }
}
