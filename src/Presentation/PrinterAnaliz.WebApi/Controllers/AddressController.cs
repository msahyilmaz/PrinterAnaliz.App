using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Features.Addresses.Command.Create;
using PrinterAnaliz.Application.Features.Addresses.Command.Delete;
using PrinterAnaliz.Application.Features.Addresses.Command.Update;
using PrinterAnaliz.Application.Features.Addresses.Queries.GetAllAddress;

namespace PrinterAnaliz.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IMediator mediator;
        public AddressController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost] 
        public async Task<IActionResult> Create([FromBody] CreateAddressesCommandRequest addressesCreateCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(addressesCreateCommandRequest));
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAddressesCommandRequest updateAddressesCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(updateAddressesCommandRequest));
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAddressesCommandRequest deleteAddressesCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(deleteAddressesCommandRequest));
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAddresses([FromQuery] GetAllAddressQueryRequest getAllAddressQueryRequest)
        {
            var response = await mediator.Send(getAllAddressQueryRequest);
            return Ok(response);
        }
    }
}
