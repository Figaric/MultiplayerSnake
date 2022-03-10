using MultiplayerSnake.Shared;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;

namespace MultiplayerSnake.Server;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IValidationManager _validationManager;

    public ValidationMiddleware(RequestDelegate next, IValidationManager validationManager)
    {
        _next = next;
        _validationManager = validationManager;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string controllerName = context.GetRouteData().Values["controller"] + "Controller";
        string actionName = context.GetRouteData().Values["action"] + "Async";
        
        var assembly = Assembly.GetAssembly(typeof(Program));

        var parameters = assembly.GetType(assembly.GetName().Name + '.' + controllerName)
            .GetMethod(actionName)
            .GetParameters();

        Type dtoType = parameters[0].ParameterType;

        var dto = assembly.CreateInstance(dtoType.Name);

        string body = await Utillities.ReadRequestBodyAsync(context.Request);
        dto = JsonConvert.DeserializeObject(body, dtoType);

        var result = await _validationManager.ValidateAsync(dto, dtoType);

        if (!result.IsValid)
        {
            var fieldErrors = result.Errors
                .Select(e => new FieldError
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Errors = fieldErrors,
            });
            await context.Response.StartAsync();

            return;
        }

        await _next(context);
    }
}