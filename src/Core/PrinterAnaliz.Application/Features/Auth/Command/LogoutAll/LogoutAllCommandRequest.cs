using MediatR;

namespace PrinterAnaliz.Application.Features.Auth.Command.LogoutAll
{
    public class LogoutAllCommandRequest:IRequest<LogoutAllCommandResponse>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
