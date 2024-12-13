using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Features.Auth.Command.ForgetPassword;
using PrinterAnaliz.Application.Features.Auth.Command.Login;
using PrinterAnaliz.Application.Features.Auth.Command.Logout;
using PrinterAnaliz.Application.Features.Auth.Command.LogoutAll;
using PrinterAnaliz.Application.Features.Auth.Command.RefreshToken;
using PrinterAnaliz.Application.Features.Auth.Command.ResetPassword;

namespace PrinterAnaliz.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommandRequest loginCommandRequest)
        {
            var response = GenericResponseModel<LoginCommandResponse>.Success(await mediator.Send(loginCommandRequest));
            return Ok(response);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommandRequest refreshTokenCommandRequest)
        {
            var response = GenericResponseModel<RefreshTokenCommandResponse>.Success(await mediator.Send(refreshTokenCommandRequest));
            return Ok(response);
        }
    
        [HttpPost]
        public async Task<IActionResult> Logout([FromBody] LogoutCommandRequest logoutCommandRequest)
        {
            var response = GenericResponseModel<LogoutCommandResponse>.Success(await mediator.Send(logoutCommandRequest));
            return Ok(response);
        }
        [HttpPost]
        [Authorize(Roles = "King")]
        public async Task<IActionResult> LogoutAll([FromBody] LogoutAllCommandRequest logoutCommandRequest)
        {
            var response = GenericResponseModel<LogoutAllCommandResponse>.Success(await mediator.Send(logoutCommandRequest));
            return Ok(response);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest forgetPasswordRequest)
        {
            var response = GenericResponseModel<ForgetPasswordResponse>.Success(await mediator.Send(forgetPasswordRequest));
            return Ok(response);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var response = GenericResponseModel<ResetPasswordResponse>.Success(await mediator.Send(resetPasswordRequest));
            return Ok(response);
        }
    }
}
