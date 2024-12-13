using FluentValidation;
using FluentValidation.Results;
using Microsoft.IdentityModel.Tokens;

namespace PrinterAnaliz.Application.Exceptions
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<Exception> GetAllExceptions(this Exception ex)
        {
            Exception currentEx = ex;
            yield return currentEx;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx;
            }
        }

        public static IEnumerable<string> GetAllExceptionAsString(this Exception ex)
        {
            Exception currentEx = ex;
            yield return currentEx.ToString();
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx.ToString();
            }
        }

        public static IEnumerable<string> GetAllExceptionMessages(this Exception ex)
        {
            Exception currentEx = ex;
            yield return currentEx.Message;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx.Message;
            }
        }
        public static List<string> GetAllExceptionMessages(this ValidationException ex)
        {
            if (ex.Errors.Any())
                return ex.Errors.Select(s=>s.ErrorMessage).ToList();
            return new List<string> { ex.Message.ToString() };
        }
        public static List<string> GetAllExceptionMessages(this SecurityTokenException ex)
        {
            return new List<string> { ex.Message.ToString() };
        }
    }
}
