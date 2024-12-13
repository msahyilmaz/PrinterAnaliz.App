using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Features.Costumers.Command.Delete;
using PrinterAnaliz.Application.Features.Costumers.Command.Update;
using PrinterAnaliz.Application.Features.Costumers.Queries.GetAllCustomer;
using PrinterAnaliz.Application.Features.Customers.Command.Create;

namespace PrinterAnaliz.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {

        private readonly IMediator mediator;

        public CustomerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "King,Admin,Editor")]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommandRequest createCustomerCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(createCustomerCommandRequest));
            return Ok(response);
        }
        [HttpPut]
        [Authorize(Roles = "King,Admin,Editor")]
        public async Task<IActionResult> Update([FromBody] UpdateCostumerCommandRequest updateCostumerCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(updateCostumerCommandRequest));
            return Ok(response);
        }
        [HttpDelete]
        [Authorize(Roles = "King,Admin,Editor")]
        public async Task<IActionResult> Delete([FromBody] DeleteCustomerCommandRequest deleteCustomerCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(deleteCustomerCommandRequest));
            return Ok(response);
        }
        [HttpGet]
        [Authorize(Roles = "King,Admin,Editor")]
        public async Task<IActionResult> GetAllCustomers([FromQuery] GetAllCustomerQueryRequest getAllCustomerQueryRequest)
        {
            var response = await mediator.Send(getAllCustomerQueryRequest);
            return Ok(response);
        }
    }
}
