using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrinterAnaliz.Application.Extensions
{
    public class GenericResponseModel<T> 
    {
        
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public int StatusCode { get; set; }
        public T ErrorMessage { get; set; }
        public int TotalRecordCount { get; set; } = 0;
        public int PageIndex { get; set; } = 0;
        public int ItemCount { get; set; } = 10;
        public string OrderBy { get; set; } = "ID DESC";
        public static GenericResponseModel<T> Fail(T errorMessage, int statusCode)
        {
            return new GenericResponseModel<T> { Succeeded = false, ErrorMessage = errorMessage, StatusCode = statusCode};
        }
        public static GenericResponseModel<T> Success(T data, int totalRecordCount = 0, int pageIndex = 0, int itemCount = 10)
        {
            return new GenericResponseModel<T> { Succeeded = true, Data = data, StatusCode = 200, TotalRecordCount = totalRecordCount, PageIndex = pageIndex, ItemCount = itemCount };
        }
        
    }
}
