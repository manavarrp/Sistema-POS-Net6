using FluentValidation;
using POS.Application.Dtos.Category.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Validators.Category
{
    public class CategoryValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("El campo Nombre no puede estar vacio")
                .NotEmpty().WithMessage("El campo Nombre no puede estar vacio");
        }
    }
}
