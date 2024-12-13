﻿using MediatR;
using PrinterAnaliz.Application.Enums;

namespace PrinterAnaliz.Application.Features.Addresses.Command.Update
{
    public class UpdateAddressesCommandRequest:IRequest<long>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long? CustomerId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string AddressDescription { get; set; }
        public string LatLng { get; set; }
        public string TaxNumber { get; set; }
        public AddressesTypes AddressType { get; set; } = AddressesTypes.Individual;
    }
}
