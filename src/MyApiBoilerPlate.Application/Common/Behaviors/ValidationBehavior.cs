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

    public ValidationBehavior(IValidator<TRequest>? validator)
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

        if (validationResult.Errors.Any())
        {
            throw new ValidationException(validationResult.Errors);
        }

        return await next(message, cancellationToken);
    }
}




