using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Extensions
{
    public class GenericQueryModel
    {
        public string Search { get; set; }
        public int PageIndex { get; set; } = 0;
        public int ItemCount { get; set; } = 10;
        public string OrderBy { get; set; } = "Id DESC";
    }
}
