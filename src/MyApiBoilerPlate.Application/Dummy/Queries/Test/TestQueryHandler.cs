using ErrorOr;
using Mediator;

namespace MyApiBoilerPlate.Application.Dummy.Queries.Test
{
    public sealed class TestQueryHandler : IRequestHandler<TestQuery, ErrorOr<string>>
    {
        public TestQueryHandler()
        {
        }

        public async ValueTask<ErrorOr<string>> Handle(TestQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return ErrorOrFactory.From($"Hello {request.Name} from the TestQueryHandler!");
        }
    }
}
