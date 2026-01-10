using FluentValidation;
using MyApiBoilerPlate.Application.Common.Constants;

namespace MyApiBoilerPlate.Application.Users.Commands.CreateUser
{
  public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
  {
    public CreateUserCommandValidator()
    {
      RuleFor(x => x.FirstName)
          .NotEmpty().WithMessage("First name is required.")
          .MaximumLength(ValidationConstants.User.FirstNameMaxLength)
          .WithMessage($"First name must not exceed {ValidationConstants.User.FirstNameMaxLength} characters.");

      RuleFor(x => x.LastName)
          .NotEmpty().WithMessage("Last name is required.")
          .MaximumLength(ValidationConstants.User.LastNameMaxLength)
          .WithMessage($"Last name must not exceed {ValidationConstants.User.LastNameMaxLength} characters.");

      RuleFor(x => x.Email)
          .NotEmpty().WithMessage("Email is required.")
          .EmailAddress().WithMessage("A valid email is required.")
          .MaximumLength(ValidationConstants.User.EmailMaxLength)
          .WithMessage($"Email must not exceed {ValidationConstants.User.EmailMaxLength} characters.");

      RuleFor(x => x.PhoneNumber)
          .NotEmpty().WithMessage("Phone number is required.")
          .Matches(ValidationConstants.User.PhoneNumberPattern)
          .WithMessage("A valid phone number is required.");

      RuleFor(x => x.DateOfBirth)
          .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");
    }
  }
}
