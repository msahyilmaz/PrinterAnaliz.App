using MediatR;

namespace PrinterAnaliz.Application.Features.Auth.Command.ForgetPassword
{
    public class ForgetPasswordRequest:IRequest<ForgetPasswordResponse>
    {
        public string EmailOrUserName { get; set; }
        public string PasswordResetUrl { get; set; }
    }
}
