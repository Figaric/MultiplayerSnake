using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace MultiplayerSnake.Server;

public class ResponseMappingFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if(context.Result is ObjectResult objectResult &&
           objectResult.Value is IResponse response &&
           response.StatusCode != HttpStatusCode.OK)
        {
            context.Result = new ObjectResult(response) { StatusCode = (int)response.StatusCode };
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    { }
}
