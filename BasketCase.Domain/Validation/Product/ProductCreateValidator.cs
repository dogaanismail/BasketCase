using BasketCase.Domain.Dto.Request.Product;
using FluentValidation;

namespace BasketCase.Domain.Validation.Product
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage(x => string.Format(ValidationMessage.RequiredField, nameof(x.Name)));
        }
    }
}
