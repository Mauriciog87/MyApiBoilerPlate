using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using MyApiBoilerPlate.API.Common;

namespace MyApiBoilerPlate.API.Controllers
{
  [ApiController]
  public class ApiController : ControllerBase
  {
    protected IActionResult Problem(List<Error> errors)
    {
      if (errors.Count is 0)
        return Problem();

      if (errors.All(error => error.Type == ErrorType.Validation))
        return ValidationProblem(errors);

      var firstError = errors[0];
      int statusCode = ErrorMapper.MapErrorToStatusCode(firstError);

      return Problem(statusCode: statusCode, title: firstError.Description);
    }

    private ActionResult ValidationProblem(List<Error> errors)
    {
      var modelStateDictionary = ErrorMapper.CreateModelStateDictionary(errors);
      return ValidationProblem(modelStateDictionary);
    }
  }
}