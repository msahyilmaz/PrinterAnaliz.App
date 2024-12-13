using MediatR;
using PrinterAnaliz.Application.Extensions;

namespace PrinterAnaliz.Application.Features.PrinterLogs.Queries.GetAllPrinterLogs
{
    public class GetAllPrinterLogsQueryRequest: GenericQueryModel, IRequest<GenericResponseModel<IList<GetAllPrinterLogsQueryResponse>>>
    {
        private string printerId;
        private string strCustomerId;
        public string PrinterId
        {
            get { return printerId.ConvertSPIdList(); }
            set { printerId = value; }
        }
        public string StrCustomerId
        {
            get { return strCustomerId.ConvertSPIdList(); }
            set { strCustomerId = value; }
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
