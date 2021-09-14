using BasketCase.Domain.Dto.Request.Product;
using BasketCase.Domain.Validation.Product;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace BasketCase.Tests.BasketCase.Domain.Tests.Validation.Product
{
    [TestFixture]
    public class ProductCreateValidatorTests : BaseTest
    {
        private ProductCreateValidator _productCreateValidator;

        [OneTimeSetUp]
        public void Setup()
        {
            _productCreateValidator = GetService<ProductCreateValidator>();
        }

        [Test]
        public void ShouldHaveErrorWhenProductNameIsNullOrEmpty()
        {
            var model = new ProductCreateRequest
            {
                Name = null
            };

            _productCreateValidator.ShouldHaveValidationErrorFor(x => x.Name, model);

            model.Name = string.Empty;
            _productCreateValidator.ShouldHaveValidationErrorFor(x => x.Name, model);
        }
    }
}
