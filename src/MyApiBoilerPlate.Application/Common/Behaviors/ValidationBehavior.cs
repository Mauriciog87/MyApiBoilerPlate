using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using Mediator;

namespace MyApiBoilerPlate.Application.Common.Behaviors;

/// <summary>
/// Pipeline behavior that validates requests using FluentValidation before they reach the handler.
/// If validation fails, returns validation errors without invoking the handler.
/// </summary>
/// <remarks>
/// This behavior integrates FluentValidation with the ErrorOr pattern for consistent error handling.
/// The constraint <c>TResponse : IErrorOr</c> ensures this behavior only applies to handlers
/// that return ErrorOr types, allowing validation errors to be returned as part of the Result pattern.
/// </remarks>
/// <typeparam name="TRequest">The type of request being validated.</typeparam>
/// <typeparam name="TResponse">The response type, must implement IErrorOr.</typeparam>
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

    List<Error> errors = validationResult.Errors
        .ConvertAll(error => Error.Validation(
            code: error.ErrorCode,
            description: error.ErrorMessage
        ));

    // NOTE: The use of (dynamic) here is the official pattern recommended by the ErrorOr library
    // for generic pipeline behaviors. Since TResponse is constrained to IErrorOr but we don't know
    // the concrete ErrorOr<T> type at compile time, we use dynamic to leverage the implicit
    // conversion from List<Error> to ErrorOr<T> that ErrorOr provides.
    // See: https://github.com/amantinband/error-or#validation-behavior-with-mediatr-and-fluentvalidation
    return (dynamic)errors;
  }
}
