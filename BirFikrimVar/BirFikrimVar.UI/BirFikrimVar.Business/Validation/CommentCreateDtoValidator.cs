using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Entity.DTOs;
using FluentValidation;

namespace BirFikrimVar.Business.Validation
{
    public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
    {
        public CommentCreateDtoValidator() {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş olamaz.")
                .MinimumLength(2).WithMessage("Yorum çok kısa.");

            RuleFor(x => x.IdeaId)
                .GreaterThan(0).WithMessage("Geçersiz fikir kimliği.");
        }
    }
}
