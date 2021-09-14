using BasketCase.Domain.Dto.Request.Product;
using FluentValidation;
using System;

namespace BasketCase.Domain.Validation.Product
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().When(prod => String.IsNullOrEmpty(prod.Name)).
                WithMessage(x => string.Format(ValidationMessage.RequiredField, nameof(x.Name)));

            RuleFor(p => p.Published).NotEmpty().WithMessage(x => string.Format(ValidationMessage.RequiredField, nameof(x.Published)));

            RuleFor(p => p.Deleted).NotEmpty().WithMessage(x => string.Format(ValidationMessage.RequiredField, nameof(x.Deleted)));
        }
    }
}
