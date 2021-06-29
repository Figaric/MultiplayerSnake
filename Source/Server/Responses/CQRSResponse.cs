using FluentValidation.Results;
using System.Collections.Generic;

namespace MultiplayerSnake.Server
{
    public record CQRSResponse
    {
        public bool Ok { get; init; } = true;

        public IList<FieldError> Errors { get; init; }

        public static CQRSResponse Succeed() => new CQRSResponse();

        public static CQRSResponse Failed(params FieldError[] errors) => new CQRSResponse { Ok = false, Errors = errors };
    }
}
