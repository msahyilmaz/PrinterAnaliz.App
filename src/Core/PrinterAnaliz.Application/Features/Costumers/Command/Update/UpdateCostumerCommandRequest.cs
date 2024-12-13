using MediatR;

namespace PrinterAnaliz.Application.Features.Costumers.Command.Update
{
    public class UpdateCostumerCommandRequest:IRequest<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string LogoImage { get; set; }
        public string AccountantId { get; set; }
    }
}
