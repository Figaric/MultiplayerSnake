using MultiplayerSnake.Shared;
using Newtonsoft.Json;
using System.Net;

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
        UserRegisterDto dto;

        string body = await Utillities.ReadRequestBodyAsync(context.Request);
        dto = JsonConvert.DeserializeObject<UserRegisterDto>(body);

        var result = await _validationManager.ValidateAsync(dto);

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