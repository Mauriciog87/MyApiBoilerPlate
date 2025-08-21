using FluentValidation;

namespace MyApiBoilerPlate.Application.Dummy.Queries.Test
{
    public class TestQueryValidator : AbstractValidator<TestQuery>
    {
        public TestQueryValidator()
        {
            RuleFor(x => x.Name).MinimumLength(5).WithMessage("Name must be at least 5 characters long.");
        }
    }
}
