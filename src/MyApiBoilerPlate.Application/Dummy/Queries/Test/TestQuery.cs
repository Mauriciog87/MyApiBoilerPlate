using ErrorOr;
using Mediator;

namespace MyApiBoilerPlate.Application.Dummy.Queries.Test
{
    public sealed record TestQuery(string Name) : IRequest<ErrorOr<string>>;
}
