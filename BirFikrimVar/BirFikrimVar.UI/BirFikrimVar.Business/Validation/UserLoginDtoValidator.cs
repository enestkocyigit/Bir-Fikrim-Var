using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Entity.DTOs;
using FluentValidation;

namespace BirFikrimVar.Business.Validation
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator() {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir e-posta giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur.");
        }
    }
}
