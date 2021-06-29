using FluentValidation;

namespace MultiplayerSnake.Server
{
    public class UserValidator : AbstractValidator<UserCreateDto>
    {
        private readonly UserManager _manager;

        public UserValidator(UserManager manager)
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Username is required")
                .Must(NotDuplicate)
                .WithMessage("This username is already taken.");

            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required");

            _manager = manager;
        }

        private bool NotDuplicate(string username)
        {
            return _manager.GetUserByName(username) == null;
        }
    }
}
