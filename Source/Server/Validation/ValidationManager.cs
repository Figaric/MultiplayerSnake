using FluentValidation;
using FluentValidation.Results;

namespace MultiplayerSnake.Server
{
    public interface IValidationManager
    {
        Task<ValidationResult> ValidateAsync<TType>(TType model);
    }

    public class ValidationManager : IValidationManager
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ValidationResult> ValidateAsync<TType>(TType model)
        {
            var validator = _serviceProvider.GetRequiredService<IValidator<TType>>();

            var validationResult = await validator.ValidateAsync(model);

            return validationResult;
        }
    }
}
