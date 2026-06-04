using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.dtos.Requests;

namespace VetCareBackend.Application.Validations
{
    public class UserRequestValidation : AbstractValidator<UserRequest>
    {
        public UserRequestValidation()
        {
            RuleFor(request => request.FirstName).MinimumLength(3).MaximumLength(15).When(request => !string.IsNullOrWhiteSpace(request.FirstName));
            RuleFor(request => request.LastName).MinimumLength(3).MaximumLength(15).When(request => !string.IsNullOrWhiteSpace(request.LastName));
            RuleFor(request => request.Dni).Length(8).When(request => !string.IsNullOrWhiteSpace(request.Dni));
            RuleFor(request => request.Email).EmailAddress().When(request => !string.IsNullOrWhiteSpace(request.Email));
            RuleFor(request => request.PhoneNumber).MinimumLength(9).MaximumLength(11).When(request => !string.IsNullOrWhiteSpace(request.PhoneNumber));
        }
    }
}
