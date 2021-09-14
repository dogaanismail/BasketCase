using BasketCase.Business.Interfaces.Product;
using BasketCase.Core.Domain.Product;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.Product;
using BasketCase.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BasketCase.Business.Interfaces.Logging;
using BasketCase.Domain.Enumerations;
using Newtonsoft.Json;

namespace BasketCase.Business.Services.Product
{
    /// <summary>
    /// ProductVariantService service implementations
    /// </summary>
    public class ProductVariantService : IProductVariantService
    {
        #region Fields
        private readonly IRepository<ProductVariant> _productVariantRepository;
        private readonly ILogService _logService;

        #endregion

        #region Ctor
        public ProductVariantService(IRepository<ProductVariant> productVariantRepository,
            ILogService logService)
        {
            _productVariantRepository = productVariantRepository;
            _logService = logService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts a product variant
        /// </summary>
        /// <param name="productVariant"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(ProductVariant productVariant)
        {
            if (productVariant == null)
                throw new ArgumentNullException(nameof(productVariant));

            await _productVariantRepository.AddAsync(productVariant);
        }

        /// <summary>
        /// Inserts product variants by using bulk
        /// </summary>
        /// <param name="productVariants"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(List<ProductVariant> productVariants)
        {
            if (productVariants == null)
                throw new ArgumentNullException(nameof(productVariants));

            await _productVariantRepository.AddRangeAsync(productVariants);
        }

        /// <summary>
        /// Inserts a product variant
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ServiceResponse<object>> CreateAsync(ProductVariantCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var serviceResponse = new ServiceResponse<object>
            {
                Success = true
            };

            try
            {
                ProductVariant variant = new()
                {
                    ProductId = request.ProductId,
                    Sku = request.Sku,
                    Barcode = request.Barcode,
                    MinStockQuantity = request.MinStockQuantity,
                    StockQuantity = request.Quantity
                };

                await CreateAsync(variant);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                _ = _logService.InsertLogAsync(LogLevel.Error, $"ProductVariantService-CreateAsync Error: model {JsonConvert.SerializeObject(request)}", ex.Message.ToString());
                serviceResponse.Success = false;
                serviceResponse.ResultCode = ResultCode.Exception;
                serviceResponse.Warnings.Add(ex.Message);
                return serviceResponse;
            }
        }

        /// <summary>
        /// Deletes a product variant
        /// </summary>
        /// <param name="productVariant"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(ProductVariant productVariant)
        {
            if (productVariant == null)
                throw new ArgumentNullException(nameof(productVariant));

            await _productVariantRepository.DeleteAsync(productVariant);
        }

        /// <summary>
        /// Gets a product variant by id
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        public virtual async Task<ProductVariant> GetByIdAsync(string variantId)
        {
            if (string.IsNullOrEmpty(variantId))
                throw new ArgumentNullException(nameof(variantId));

            return await _productVariantRepository.GetByIdAsync(variantId);
        }

        /// <summary>
        /// Gets a product by variant id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ProductVariant> GetByProductIdAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                throw new ArgumentNullException(nameof(productId));

            return await _productVariantRepository.GetAsync(x => x.ProductId == productId);
        }

        /// <summary>
        /// Gets product variant lists
        /// </summary>
        /// <returns></returns>
        public virtual List<ProductVariant> GetVariants()
        {
            return _productVariantRepository.Get().ToList();
        }

        #endregion
    }
}
