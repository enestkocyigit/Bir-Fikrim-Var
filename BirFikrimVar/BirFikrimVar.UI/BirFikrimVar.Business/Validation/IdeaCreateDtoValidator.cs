using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Entity.DTOs;
using FluentValidation;

namespace BirFikrimVar.Business.Validation
{
    public class IdeaCreateDtoValidator : AbstractValidator<IdeaCreateDto>
    {
        public IdeaCreateDtoValidator() {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık zorunludur.")
                .MinimumLength(3).WithMessage("İçerik en az 10 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik zorunludur.")
                .MinimumLength(5).WithMessage("İçerik en az 5 karakter olmalıdır.");
        }
    }
}
