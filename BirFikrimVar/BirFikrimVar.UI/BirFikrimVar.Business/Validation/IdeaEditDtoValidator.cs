using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Entity.DTOs;
using FluentValidation;

namespace BirFikrimVar.Business.Validation
{
    public class IdeaEditDtoValidator : AbstractValidator<IdeaEditDto>
    {
        public IdeaEditDtoValidator() {

            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık zorunludur.")
                .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik zorunludur.")
                .MinimumLength(2).WithMessage("İçerik en az 2 karakter olmalıdır.");
        }
    }
}
