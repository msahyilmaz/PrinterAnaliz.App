using FluentValidation;
using MediatR;

namespace PrinterAnaliz.Application.Beheviors
{
    public class FluentValidationBehevior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validator;

        public FluentValidationBehevior(IEnumerable<IValidator<TRequest>> _validator)
        {
            validator = _validator;
        }
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failtures = validator
                .Select(v => v.Validate(context))
                .SelectMany(v => v.Errors)
                .GroupBy(v => v.ErrorMessage)
                .Select(s => s.First())
                .Where(x => x != null)
                .ToList();
            if (failtures.Any())
                throw new ValidationException(failtures);

            return next();
        }
    }
}
