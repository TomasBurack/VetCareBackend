using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(ur => ur.FirstName).NotEmpty().MinimumLength(3).MaximumLength(15);
            RuleFor(ur => ur.LastName).NotEmpty().MinimumLength(3).MaximumLength(15);
            RuleFor(ur => ur.Dni).NotEmpty().Length(8);
            RuleFor(ur => ur.Email).NotEmpty().EmailAddress();
            RuleFor(ur => ur.Password).NotEmpty().MinimumLength(10);
            RuleFor(ur => ur.PhoneNumber).MinimumLength(9).MaximumLength(11);
        }
    }
}
