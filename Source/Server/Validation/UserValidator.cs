﻿using FluentValidation;
using MultiplayerSnake.Server.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerSnake.Server.Validation
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .Must(u => u.Length >= 3)
                .WithMessage("Username must be at least 3 characters long");

            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .Must(u => u.Length >= 3)
                .WithMessage("Password must be at least 3 characters long");
        }
    }

    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Username is required.");

            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}
