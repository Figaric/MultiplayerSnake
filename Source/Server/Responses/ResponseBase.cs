using FluentValidation.Results;
using System.Collections.Generic;
using System.Net;

namespace MultiplayerSnake.Server.Responses
{
    public record ResponseBase
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

        public IList<FieldError> Errors { get; init; }

        public static ResponseBase Succeed => new ResponseBase();

        public static ResponseBase Failed(params FieldError[] errors)
            => new ResponseBase { StatusCode = HttpStatusCode.BadRequest, Errors = errors };
    }

    public record ResponseBase<TData> : ResponseBase
    {
        public TData Data { get; init; }

        public static ResponseBase<TData> Succeed(TData data)
            => new() { Data = data };
    }
}
