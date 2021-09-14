using BasketCase.Domain.Dto.Request.ShoppingCart;
using FluentValidation;
using System;

namespace BasketCase.Domain.Validation.ShoppingCart
{
    public class AddToCartValidator : AbstractValidator<AddToCartRequest>
    {
        public AddToCartValidator()
        {
            RuleFor(p => p.Quantity).NotNull().WithMessage(x => string.Format(ValidationMessage.RequiredField, nameof(x.Quantity)))
                .GreaterThan(0)
                .WithMessage(ValidationMessage.GreaterThan(0));

            RuleFor(p => p.ProductId).NotEmpty().WithMessage(x => string.Format(ValidationMessage.RequiredField, nameof(x.ProductId)));

            RuleFor(p => p.ProductVariantId).NotEmpty().WithMessage(x => string.Format(ValidationMessage.RequiredField, nameof(x.ProductVariantId)));
        }
    }
}
