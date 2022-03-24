using FluentValidation;
using MezuniyetProjesi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Validator
{
    public class RegisterValidator : AbstractValidator<RegisterModel>
    {
        public RegisterValidator()
        {
            RuleFor(r => r.Username).NotEmpty().NotNull();
            RuleFor(r => r.Email).EmailAddress().NotEmpty().NotNull();
            RuleFor(r => r.Password).NotEmpty().NotNull().Length(8, 20).WithMessage("Şifre en az 8, en fazla 20 karakter olmalıdır.");
        }
    }
}
