using MediatR;

namespace PrinterAnaliz.Application.Features.Auth.Command.ResetPassword
{
    public class ResetPasswordRequest:IRequest<ResetPasswordResponse>
    {
        public string ResetToken { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
