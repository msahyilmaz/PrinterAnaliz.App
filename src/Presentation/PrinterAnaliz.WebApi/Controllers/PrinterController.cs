using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Features.Printers.Command.Create;
using PrinterAnaliz.Application.Features.Printers.Command.Delete;
using PrinterAnaliz.Application.Features.Printers.Command.Update;
using PrinterAnaliz.Application.Features.Printers.Queries.GetAllPrinter;
using PrinterAnaliz.Application.Features.Users.Commands.User.Delete;

namespace PrinterAnaliz.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PrinterController : ControllerBase
    {
        private readonly IMediator mediator;
        public PrinterController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "King,Admin,Editor")]
        public async Task<IActionResult> Create([FromBody] CreatePrinterCommandRequest createPrinterCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(createPrinterCommandRequest));
            return Ok(response);
        }
        [HttpPut]
        [Authorize(Roles = "King,Admin,Editor")]
        public async Task<IActionResult> Update([FromBody] UpdatePrinterCommandRequest updatePrinterCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(updatePrinterCommandRequest));
            return Ok(response);
        }
        [HttpDelete]
        [Authorize(Roles = "King,Admin,Editor")]
        public async Task<IActionResult> Delete([FromBody] DeletePrinterCommandRequest deleteUserCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(deleteUserCommandRequest));
            return Ok(response);
        }
        [HttpGet] 
        public async Task<IActionResult> GetAllPrinters([FromQuery] GetAllPrinterRequest getAllPrinterRequest)
        {
            var response = await mediator.Send(getAllPrinterRequest);
            return Ok(response);
        }
    }
}
