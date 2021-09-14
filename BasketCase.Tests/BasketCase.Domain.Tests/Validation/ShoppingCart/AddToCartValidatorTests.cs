using BasketCase.Domain.Dto.Request.ShoppingCart;
using BasketCase.Domain.Validation.ShoppingCart;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace BasketCase.Tests.BasketCase.Domain.Tests.Validation.ShoppingCart
{
    [TestFixture]
    public class AddToCartValidatorTests : BaseTest
    {
        private AddToCartValidator _addToCartValidator;

        [OneTimeSetUp]
        public void Setup()
        {
            _addToCartValidator = GetService<AddToCartValidator>();
        }

        [Test]
        public async Task ShouldHaveErrorWhenQuantityLessThanOrEqualToZeroAsync()
        {
            var model = new AddToCartRequest
            {
                Quantity = 0,
                ProductId = "123",
                ProductVariantId = "1313"
            };

            var result = await _addToCartValidator.ValidateAsync(model);
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public async Task ShouldHaveErrorWhenProductIdNullOrEmpty()
        {
            var model = new AddToCartRequest
            {
                Quantity = 1,
                ProductId = "",
                ProductVariantId = "1313"
            };

            var result = await _addToCartValidator.ValidateAsync(model);
            result.IsValid.Should().BeFalse();

            model.ProductId = string.Empty;

            result = await _addToCartValidator.ValidateAsync(model);
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public async Task ShouldHaveErrorWhenProductVariantIdNullOrEmpty()
        {
            var model = new AddToCartRequest
            {
                Quantity = 1,
                ProductId = "123",
                ProductVariantId = ""
            };

            var result = await _addToCartValidator.ValidateAsync(model);
            result.IsValid.Should().BeFalse();

            model.ProductVariantId = string.Empty;
            result = await _addToCartValidator.ValidateAsync(model);
            result.IsValid.Should().BeFalse();
        }
    }
}
