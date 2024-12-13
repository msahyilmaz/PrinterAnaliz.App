using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Features.Auth.Command.Login
{
    public class LoginCommandRequest : IRequest<LoginCommandResponse>
    {
        [DefaultValue("xxx@yyy.zzz")]
        public string UserName { get; set; } 
        [DefaultValue("Password:1923")]
        public string Password { get; set; }
    }
}
