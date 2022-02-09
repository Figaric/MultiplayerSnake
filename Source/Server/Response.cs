﻿using System.Collections.Generic;
using System.Net;

namespace MultiplayerSnake.Server
{
    public interface IResponse
    {
        HttpStatusCode StatusCode { get; }

        static IResponse Succeed(HttpStatusCode statusCode = HttpStatusCode.OK)
            => throw new NotImplementedException();
    }

    public class ResponseBase : IResponse
    {
        public HttpStatusCode StatusCode { get; protected set; }

        public static IResponse Succeed(HttpStatusCode statusCode = HttpStatusCode.OK)
            => new ResponseBase { StatusCode = statusCode };
    }

    public class ResponseFail<TError> : ResponseBase, IResponse
    {
        public IEnumerable<TError> Errors { get; private set; }

        public static IResponse Failed(HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new ResponseFail<TError> { StatusCode = statusCode };

        public static IResponse Failed(TError error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new ResponseFail<TError> { Errors = new List<TError> { error }, StatusCode = statusCode };

        public static IResponse Failed(IEnumerable<TError> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new ResponseFail<TError> { Errors = errors, StatusCode = statusCode };
    }

    public class ResponseData<TData> : ResponseBase, IResponse
    {
        public TData Data { get; private set; }

        public static IResponse Succeed(TData data, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new ResponseData<TData> { Data = data, StatusCode = statusCode };
    }
}