using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using Mediator;

namespace MyApiBoilerPlate.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async ValueTask<TResponse> Handle(
        TRequest message,
        MessageHandlerDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (_validator is null)
            return await next(message, cancellationToken);

        ValidationResult validationResult = await _validator.ValidateAsync(message, cancellationToken);

        if (validationResult.IsValid)
            return await next(message, cancellationToken);

        List<ErrorOr.Error> errors = validationResult.Errors
            .ConvertAll(error => ErrorOr.Error.Validation(
                code: error.ErrorCode,
                description: error.ErrorMessage
            ));

        return (dynamic)errors;
    }
}
