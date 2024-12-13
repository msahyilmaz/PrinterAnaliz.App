using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Features.Auth.Command.Logout
{
    public class LogoutCommandRequest:IRequest<LogoutCommandResponse>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
