using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Features.PrinterLogs.Command.Create;
using PrinterAnaliz.Application.Features.PrinterLogs.Queries.GetAllPrinterLogs;

namespace PrinterAnaliz.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PrinterLogController : ControllerBase
    {
        private readonly IMediator mediator;
        public PrinterLogController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePrinterLogCommandRequest createPrinterLogCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(createPrinterLogCommandRequest));
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPrinterLogs([FromQuery] GetAllPrinterLogsQueryRequest getAllPrinterLogsQueryRequest)
        {
            var response = await mediator.Send(getAllPrinterLogsQueryRequest);
            return Ok(response);
        }
    }
}
