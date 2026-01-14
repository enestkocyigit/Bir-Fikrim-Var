using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Entity.DTOs;
using FluentValidation;

namespace BirFikrimVar.Business.Validation
{
    public class CommentEditDtoValidator : AbstractValidator<CommentEditDto>
    {
        public CommentEditDtoValidator() {
            RuleFor(c => c.Content)
                .NotEmpty().WithMessage("Yorum boş olamaz.")
                .MaximumLength(500).WithMessage("Yorum en fazla 500 karakter olabilir.");
        }
    }
}
