using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : CQRSResponse, new()
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationBehaviour() { }

        public ValidationBehaviour(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validator is null)
                return await next();

            var result = await _validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                new TResponse
                {
                    Ok = false,
                    Errors = result.Errors
                        .Select(e => new FieldError
                        { 
                            Field = e.PropertyName, 
                            Message = e.ErrorMessage 
                        })
                        .ToList()
                };
            }

            return await next();
        }
    }
}
