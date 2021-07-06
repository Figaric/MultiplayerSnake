using System.Collections.Generic;
using System.Net;

namespace MultiplayerSnake.Server
{
    public record ResponseBase
    {
        public HttpStatusCode StatusCode { get; init; }

        public IList<FieldError> Errors { get; init; }

        public static ResponseBase Succeed(HttpStatusCode statusCode = HttpStatusCode.OK)
            => new ResponseBase() { StatusCode = statusCode };

        public static ResponseBase Failed(params FieldError[] errors)
            => new ResponseBase() { Errors = errors, StatusCode = HttpStatusCode.BadRequest };

        public static ResponseBase Failed(HttpStatusCode statusCode, params FieldError[] errors)
            => new ResponseBase { StatusCode = statusCode, Errors = errors };
    }

    public record ResponseBase<TData> : ResponseBase
    {
        public TData Data { get; init; }

        public static ResponseBase<TData> Succeed(TData data, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new() { Data = data, StatusCode = statusCode };
    }
}
