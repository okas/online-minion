using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineMinion.Data.Exceptions;

namespace OnlineMinion.RestApi;

[ApiExplorerSettings(IgnoreApi = true)]
[Route("/error")]
public class ErrorsController : ControllerBase
{
    [HttpGet]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        var objectResult = exception switch
        {
            ConflictException ex => ValidationProblem(
                ex.Message,
                statusCode: StatusCodes.Status409Conflict,
                modelStateDictionary: CreateModelState(ex.Errors)
            ),
            _ => Problem(title: exception?.Message, statusCode: StatusCodes.Status500InternalServerError),
        };

        return objectResult;
    }

    private static ModelStateDictionary CreateModelState(IEnumerable<ConflictException.ErrorDescriptor> input)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var errorDetail in input.SelectMany(e => e.Details))
        {
            foreach (var error in errorDetail.Value)
            {
                modelStateDictionary.AddModelError(errorDetail.Key, error);
            }
        }

        return modelStateDictionary;
    }
}
