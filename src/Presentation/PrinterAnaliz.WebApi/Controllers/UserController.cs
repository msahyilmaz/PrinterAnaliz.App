using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Features.Users.Commands.User.Create;
using PrinterAnaliz.Application.Features.Users.Commands.User.Delete;
using PrinterAnaliz.Application.Features.Users.Commands.User.Update;
using PrinterAnaliz.Application.Features.Users.Queries.GetAllUserRoles;
using PrinterAnaliz.Application.Features.Users.Queries.GetAllUsers;

namespace PrinterAnaliz.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "King,Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQueryRequest getAllUsersQueryRequest)
        {
             
          //  var response = GenericResponseModel<IList<GetAllUsersQueryResponse>>.Success();
            return Ok(await mediator.Send(getAllUsersQueryRequest));
        }
        [HttpGet]
        [Authorize(Roles = "King,Admin")]
        public async Task<IActionResult> GetAllUserRoles([FromQuery] GetAllUserRolesQueryRequest getAllUserRolesQueryRequest)
        {
            var response = GenericResponseModel<IList<GetAllUserRolesQueryResponse>>.Success(await mediator.Send(getAllUserRolesQueryRequest));
            return Ok(response);
        }
        [HttpPost]
        [Authorize(Roles = "King,Admin")]
        public async Task<IActionResult> Create([FromForm] CreateUserCommandRequest createUserCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(createUserCommandRequest));
            return Ok(response);
        }
        [HttpPut]
        [Authorize(Roles = "King,Admin")]
        public async Task<IActionResult> Update([FromForm] UpdateUserCommandRequest updateUserCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(updateUserCommandRequest));
            return Ok(response);

        }
        [HttpDelete]
        [Authorize(Roles = "King,Admin")]
        public async Task<IActionResult> Delete([FromBody] DeleteUserCommandRequest deleteUserCommandRequest)
        {
            var response = GenericResponseModel<long>.Success(await mediator.Send(deleteUserCommandRequest));
            return Ok(response);
        }

    }
}
