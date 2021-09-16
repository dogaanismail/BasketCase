using BasketCase.Business.Interfaces.Basket;
using BasketCase.Business.Interfaces.Product;
using BasketCase.Core.Domain.Product;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.ShoppingCart;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using ProductEntity = BasketCase.Core.Domain.Product.Product;

namespace BasketCase.Tests.DevPlatform.Services.Tests.ShoppingCart
{
    [TestFixture]
    public class ShoppingCartServiceTests : ServiceTest
    {
        private IShoppingCartService _shoppingCartService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _shoppingCartService = GetService<IShoppingCartService>();
        }

        [Test]
        public async Task ItShouldAddToCart()
        {
            var result = await GetResult(published: true, deleted: false);

            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task ItShouldReturnErrorWhenQuantityIsOutOfStock()
        {
            var result = await GetResult(variantQuantity: 2, cartQuantity: 5);

            result.Warnings.Count.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task ItShouldReturnErrorWhenProductIsDeleted()
        {
            var result = await GetResult(deleted: true);

            result.Warnings.Count.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task ItShouldReturnErrorWhenProductIsNotPublished()
        {
            var result = await GetResult(published: false);

            result.Warnings.Count.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task ItShouldReturnErrorWhenCartQuantityIsZero()
        {
            var result = await GetResult(cartQuantity: 0);

            result.Warnings.Count.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task ItShouldReturnErrorWhenPriceIsZero()
        {
            var result = await GetResult(newPrice:0);

            result.Warnings.Count.Should().BeGreaterThan(0);
        }

        #region Private Methods
        private async Task<ServiceResponse<object>> GetResult(bool published = true,
            bool deleted = false,
            int variantQuantity = 10, int cartQuantity = 5,
            decimal newPrice = 50)
        {
            var product = new ProductEntity
            {
                Name = "example for test1",
                ShortDescription = "example for test1",
                FullDescription = "example for test1",
                Title = "exampe for test1",
                OldPrice = 100,
                NewPrice = newPrice,
                Deleted = deleted,
                Published = published
            };

            await GetService<IProductService>().CreateAsync(product);

            var getProduct = GetService<IProductService>().Get().OrderByDescending(x => x.CreatedAt).FirstOrDefault();

            var productVariant = new ProductVariant
            {
                ProductId = getProduct.Id,
                Sku = "XSSIZE",
                Barcode = "example",
                MinStockQuantity = 2,
                StockQuantity = variantQuantity
            };

            await GetService<IProductVariantService>().CreateAsync(productVariant);

            var getVariant = await GetService<IProductVariantService>().GetByProductIdAsync(getProduct.Id);

            AddToCartRequest request = new()
            {
                ProductId = getProduct.Id,
                Quantity = cartQuantity,
                ProductVariantId = getVariant.First().Id
            };

            var result = await _shoppingCartService.AddToCartAsync(request);

            return result;
        }
        #endregion
    }
}
