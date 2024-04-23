using ErrorOr;
using FluentValidation;
using MediatR;

namespace GymManagement.Application.Common.Behaviours
{
    public class ValdiationBehaviour<TRequest, TResponse>(IValidator<TRequest>? validator = null)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse: IErrorOr
    {
        private readonly IValidator<TRequest>? _validator = validator;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(_validator is null)
            {
                return await next();
            }

            var validateResult = await _validator.ValidateAsync(request, cancellationToken);
            if (validateResult.IsValid)
            {
                return await next();
            }

            var errors = validateResult.Errors
                .ConvertAll(error => Error.Validation(code: error.ErrorCode, description: error.ErrorMessage));

            return (dynamic)errors;
        }
    }
}
