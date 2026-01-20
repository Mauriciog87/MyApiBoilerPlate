using FluentValidation;
using MyApiBoilerPlate.Application.Common.Constants;

namespace MyApiBoilerPlate.Application.Users.Queries.GetAllUsers
{
  public sealed class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
  {
    public GetAllUsersQueryValidator()
    {
      RuleFor(x => x.Page)
          .GreaterThanOrEqualTo(PaginationConstants.MinPageNumber)
          .WithMessage($"Page number must be at least {PaginationConstants.MinPageNumber}.");

      RuleFor(x => x.PageSize)
          .GreaterThanOrEqualTo(PaginationConstants.MinPageSize)
          .WithMessage($"Page size must be at least {PaginationConstants.MinPageSize}.")
          .LessThanOrEqualTo(PaginationConstants.MaxPageSize)
          .WithMessage($"Page size must not exceed {PaginationConstants.MaxPageSize}.");

      RuleFor(x => x.SortBy)
          .MaximumLength(PaginationConstants.SortByMaxLength)
          .WithMessage($"Sort field name must not exceed {PaginationConstants.SortByMaxLength} characters.")
          .When(x => !string.IsNullOrEmpty(x.SortBy));
    }
  }
}
