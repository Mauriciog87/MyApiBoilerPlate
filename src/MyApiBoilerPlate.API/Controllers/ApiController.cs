using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

      HttpContext.Items["errors"] = errors;
      var firstError = errors[0];

      return Problem(firstError);
    }

    private ObjectResult Problem(Error error)
    {
      int statusCode = error.Type switch
      {
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
      };

      return Problem(statusCode: statusCode, title: error.Description);
    }

    private ActionResult ValidationProblem(List<Error> errors)
    {
      ModelStateDictionary modelStateDictonary = new();

      foreach (Error error in errors)
      {
        modelStateDictonary.AddModelError(error.Code, error.Description);
      }

      return ValidationProblem(modelStateDictonary);
    }
  }
}