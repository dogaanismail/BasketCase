using BasketCase.Business.Interfaces.Basket;
using BasketCase.Core.Domain.ShoppingCart;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.ShoppingCart;
using BasketCase.Repository.Generic;
using System;
using System.Threading.Tasks;

namespace BasketCase.Business.Services.ShoppingCart
{
    /// <summary>
    /// Shopping cart service implementations
    /// </summary>
    public class ShoppingCartService : ServiceExecute, IShoppingCartService
    {
        #region Fields
        private readonly IRepository<ShoppingCartItem> _shoppingCartRepository;

        #endregion

        #region Ctor
        public ShoppingCartService(IRepository<ShoppingCartItem> shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Adds a product variant for a shopping cart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ServiceResponse<object>> AddToCartAsync(AddToCartRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
