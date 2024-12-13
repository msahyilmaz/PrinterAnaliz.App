using MediatR;
using PrinterAnaliz.Application.Extensions;

namespace PrinterAnaliz.Application.Features.Printers.Queries.GetAllPrinter
{
    public class GetAllPrinterRequest : GenericQueryModel, IRequest<GenericResponseModel<IList<GetAllPrinterResponse>>>
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
            set { strCustomerId = value;  }
        }
        public string IsPrinterActive { get; set; }
        public string IsClientActive { get; set; }
    }
    
}
