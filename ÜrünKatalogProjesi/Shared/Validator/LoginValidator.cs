using FluentValidation;
using MezuniyetProjesi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Validator
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(l => l.Email).EmailAddress().NotEmpty().NotNull();
            RuleFor(l => l.Password).NotEmpty().NotNull().Length(8, 20).WithMessage("Şifre en az 8, en fazla 20 karakter olmalıdır.");;

        }
    }
}
