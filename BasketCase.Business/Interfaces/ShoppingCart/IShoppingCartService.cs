using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.ShoppingCart;
using System.Threading.Tasks;

namespace BasketCase.Business.Interfaces.Basket
{
    /// <summary>
    /// IBasketService implementations
    /// </summary>
    public interface IShoppingCartService
    {
        /// <summary>
        /// Adds a product variant for a shopping cart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResponse<object>> AddToCartAsync(AddToCartRequest request);
    }
}
