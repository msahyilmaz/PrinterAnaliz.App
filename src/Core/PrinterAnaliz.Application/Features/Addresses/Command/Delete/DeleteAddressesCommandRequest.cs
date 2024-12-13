using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Features.Addresses.Command.Delete
{
    public class DeleteAddressesCommandRequest:IRequest<long>
    {
        public long Id { get; set; }
    }
}
