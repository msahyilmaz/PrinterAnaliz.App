using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Features.Printers.Queries.GetAllPrinter;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Interfaces.Repositories
{
    public interface IPrinterRepository : IGenericRepository<Printer>
    {
        Task<IList<GetAllPrinterResponseModel>> GetAllPrinters(string sp, SqlParameter[] sqlParameters);
    }
}
