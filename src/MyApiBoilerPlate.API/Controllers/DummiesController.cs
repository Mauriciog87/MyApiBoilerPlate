using Mediator;
using Microsoft.AspNetCore.Mvc;
using MyApiBoilerPlate.Application.Dummy.Queries.Test;

namespace MyApiBoilerPlate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummiesController : ApiController
    {
        private readonly IMediator _mediator;

        public DummiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string value)
        {
            ValueTask<ErrorOr.ErrorOr<string>> result = _mediator.Send(new TestQuery(value));

            return result.Result.Match(Ok, Problem);
        }
    }
}