using BasketCase.Business.Interfaces.Basket;
using BasketCase.Business.Interfaces.Logging;
using BasketCase.Core.Configuration.Settings.ShoppingCart;
using BasketCase.Core.Domain.Product;
using BasketCase.Core.Domain.ShoppingCart;
using BasketCase.Core.Events;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.ShoppingCart;
using BasketCase.Domain.Enumerations;
using BasketCase.Repository.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductEntity = BasketCase.Core.Domain.Product.Product;

namespace BasketCase.Business.Services.ShoppingCart
{
    /// <summary>
    /// Shopping cart service implementations
    /// </summary>
    public class ShoppingCartService : ServiceExecute, IShoppingCartService
    {
        #region Fields
        private readonly IRepository<ShoppingCartItem> _shoppingCartRepository;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<ProductVariant> _productVariantRepository;
        private readonly ILogService _logService;
        private readonly IEventPublisher _eventPublisher;
        //private readonly ShoppingCartSettings _shoppingCartSettings;
        #endregion

        #region Ctor
        public ShoppingCartService(IRepository<ShoppingCartItem> shoppingCartRepository,
            IRepository<ProductEntity> productRepository,
            IRepository<ProductVariant> productVariantRepository,
            ILogService logService,
            IEventPublisher eventPublisher
            /*ShoppingCartSettings shoppingCartSettings*/)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
            _productVariantRepository = productVariantRepository;
            _logService = logService;
            _eventPublisher = eventPublisher;
            //_shoppingCartSettings = shoppingCartSettings;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Adds a product variant for a shopping cart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ServiceResponse<object>> AddToCartAsync(AddToCartRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var serviceResponse = new ServiceResponse<object>
            {
                Success = true
            };

            try
            {
                var warnings = new List<string>();

                var product = await _productRepository.GetByIdAsync(request.ProductId);

                if (product == null)
                    throw new ArgumentNullException(nameof(product));

                var productVariant = await _productVariantRepository.GetByIdAsync(request.ProductVariantId);

                if (productVariant == null)
                    throw new ArgumentNullException(nameof(product));

                var shoppingCartItem = await FindShoppingCartItemInTheCart(product.Id, productVariant.Id);

                if (shoppingCartItem != null)
                {
                    var oldQuantity = shoppingCartItem.Quantity;
                    int newQuantity = oldQuantity + request.Quantity;

                    warnings.AddRange(GetShoppingCartItemWarnings(product, productVariant, newQuantity, true));

                    if (warnings.Any())
                    {
                        _ = _logService.InsertLogAsync(LogLevel.Error, $"ShoppingCartService-AddToCartAsync Error ProductId: {product.Id} " +
                           $"VariantId:{productVariant.Id}", JsonConvert.SerializeObject(warnings));
                        return ServiceResponse((object)null, warnings);
                    }

                    shoppingCartItem.Quantity = newQuantity;
                    shoppingCartItem.UpdatedAt = DateTime.UtcNow;
                    await _shoppingCartRepository.UpdateAsync(shoppingCartItem.Id, shoppingCartItem);
                    await _eventPublisher.EntityUpdatedAsync(shoppingCartItem);
                    return serviceResponse;
                }
                else
                {
                    warnings.AddRange(GetShoppingCartItemWarnings(product, productVariant, request.Quantity, true));

                    if (warnings.Any())
                    {
                        _ = _logService.InsertLogAsync(LogLevel.Error, $"ShoppingCartService-AddToCartAsync Error ProductId: {product.Id} " +
                           $"VariantId:{productVariant.Id}", JsonConvert.SerializeObject(warnings));
                        return ServiceResponse((object)null, warnings);
                    }

                    shoppingCartItem = new ShoppingCartItem
                    {
                        ProductId = request.ProductId,
                        VariantId = request.ProductVariantId,
                        Quantity = request.Quantity,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _shoppingCartRepository.AddAsync(shoppingCartItem);
                    await _eventPublisher.EntityInsertedAsync(shoppingCartItem);
                    return serviceResponse;
                }
            }
            catch (Exception ex)
            {
                _ = _logService.InsertLogAsync(LogLevel.Error, $"ShoppingCartService-AddToCartAsync Error: model {JsonConvert.SerializeObject(request)}", ex.Message.ToString());
                serviceResponse.Success = false;
                serviceResponse.ResultCode = ResultCode.Exception;
                serviceResponse.Warnings.Add(ex.Message);
                return serviceResponse;
            }
        }
        #endregion

        #region Private Methods
        private List<string> GetShoppingCartItemWarnings(ProductEntity product, ProductVariant productVariant,
            int quantity = 1,
            bool getStandardWarnings = true,
            bool getVariantStockWarnings = true)
        {
            var warnings = new List<string>();

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (productVariant == null)
                throw new ArgumentNullException(nameof(productVariant));

            if (getStandardWarnings)
                warnings.AddRange(GetRequiredProductWarnings(product));

            if (getVariantStockWarnings)
                warnings.AddRange(GetVariantStockWarnings(productVariant, quantity));

            return warnings;
        }


        /// <summary>
        /// Gets standart warning for shopping cart item
        /// </summary>
        /// <param name="product"></param>
        /// <param name="productVariant"></param>
        /// <returns>Returns standart warnings</returns>
        private IList<string> GetRequiredProductWarnings(ProductEntity product)
        {
            var warnings = new List<string>();

            if (product.Deleted)
                warnings.Add($"Product was deleted!");

            if (!product.Published)
                warnings.Add("Product is not published!");

            if (product.NewPrice < 1)
                warnings.Add("Product price is less than 1!");

            return warnings;
        }

        private IList<string> GetVariantStockWarnings(ProductVariant productVariant, int quantity = 1)
        {
            var warnings = new List<string>();

            //if (_shoppingCartSettings.MinStockQuantityControl)
            //{
            //    if (quantity > (productVariant.StockQuantity - productVariant.MinStockQuantity))
            //    {
            //        warnings.Add("Product is out of stock!");
            //        return warnings;
            //    }
            //}

            if (productVariant.StockQuantity < quantity)
                warnings.Add("Product is out of stock!");

            return new List<string> { "" };
        }

        /// <summary>
        /// Finds item in shopping cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productVariantId"></param>
        /// <returns></returns>
        private async Task<ShoppingCartItem> FindShoppingCartItemInTheCart(string productId, string productVariantId)
        {
            return await _shoppingCartRepository.GetAsync(item => item.ProductId == productId && item.VariantId == productVariantId);
        }

        #endregion
    }
}
