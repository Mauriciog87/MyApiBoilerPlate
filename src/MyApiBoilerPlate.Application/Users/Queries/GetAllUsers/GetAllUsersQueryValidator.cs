using FluentValidation;
using MyApiBoilerPlate.Application.Common.Constants;

namespace MyApiBoilerPlate.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
    {
        public GetAllUsersQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(PaginationConstants.DefaultPageNumber)
                .WithMessage($"Page must be greater than or equal to {PaginationConstants.DefaultPageNumber}.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(PaginationConstants.MinPageSize)
                .WithMessage($"Page size must be greater than or equal to {PaginationConstants.MinPageSize}.")
                .LessThanOrEqualTo(PaginationConstants.MaxPageSize)
                .WithMessage($"Page size cannot exceed {PaginationConstants.MaxPageSize}.");
        }
    }
}