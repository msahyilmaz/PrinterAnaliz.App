using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterAnaliz.Application.Exceptions
{
    public class BaseException:Exception
    {
        public BaseException() : base()
        {
        }
        public BaseException(string message) : base(message)
        {
        }
        public BaseException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
