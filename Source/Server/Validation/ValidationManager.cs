using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using MultiplayerSnake.Shared;

namespace MultiplayerSnake.Server
{
    public interface IValidationManager
    {
        Task<ValidationResult> ValidateAsync<TType>(TType model);

        Task<ValidationResult> ValidateAsync(object model, Type type);
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

        public async Task<ValidationResult> ValidateAsync(object model, Type type)
        {
            Type genericValidator = typeof(IValidator<>);
            Type genericValidatorType = genericValidator.MakeGenericType(type);

            var validator = (IValidator)_serviceProvider.GetRequiredService(genericValidatorType);

            var validationResult = await validator.ValidateAsync(new ValidationContext<object>(model));

            return validationResult;
        }
    }
}
