﻿using MediatR;
using Microsoft.Data.SqlClient;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;
using PrinterAnaliz.Application.Interfaces.Repositories;

namespace PrinterAnaliz.Application.Features.Printers.Queries.GetAllPrinter
{
    public class GetAllPrinterRequestHandler : IRequestHandler<GetAllPrinterRequest, GenericResponseModel<IList<GetAllPrinterResponse>>>
    {
        private IPrinterRepository rpPrinter;
        private IGenericAutoMapper mapper;
        public GetAllPrinterRequestHandler(IPrinterRepository rpPrinter, IGenericAutoMapper mapper)
        {
            this.rpPrinter = rpPrinter;
            this.mapper = mapper;
        }
        public async Task<GenericResponseModel<IList<GetAllPrinterResponse>>> Handle(GetAllPrinterRequest request, CancellationToken cancellationToken)
        {
            var result = new GenericResponseModel<IList<GetAllPrinterResponse>>();
            var isPrinterActive = request.IsPrinterActive switch { "0" => 0, "1" => 1, _ => (object)DBNull.Value };
            var isClientActive = request.IsClientActive switch { "0" => 0, "1" => 1, _ => (object)DBNull.Value };

            SqlParameter[] sp_Printer_getAllParameters = {
            var printers = await rpPrinter.GetAllPrinters("EXECUTE Sp_Printer_getAll @Search,@PrinterId,@StrCustomerId,@IsPrinterActive,@IsClientActive, @pageIndex, @ItemCount, @ItemOrderBy", sp_Printer_getAllParameters);

            var map = mapper.Map<GetAllPrinterResponse, GetAllPrinterResponseModel>(printers);
            result.Data = map;
            result.Succeeded = true;
            result.TotalRecordCount = printers.FirstOrDefault()?.TotalRecordCount ?? 0;
            result.PageIndex = request.PageIndex;
            result.ItemCount = request.ItemCount;
            result.OrderBy = request.OrderBy;

            return result;

        }
    }
}