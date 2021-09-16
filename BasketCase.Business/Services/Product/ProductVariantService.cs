using BasketCase.Business.Interfaces.Product;
using BasketCase.Core.Domain.Product;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.Product;
using BasketCase.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BasketCase.Business.Interfaces.Logging;
using BasketCase.Domain.Enumerations;
using Newtonsoft.Json;
using MongoDB.Bson;
using BasketCase.Domain.Dto.Response.Product;
using BasketCase.Core.Infrastructure.Mapper;
using System.Linq;
using System.Linq.Expressions;

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
                    Id = ObjectId.GenerateNewId().ToString(),
                    ProductId = request.ProductId,
                    Sku = request.Sku,
                    Barcode = request.Barcode,
                    MinStockQuantity = request.MinStockQuantity,
                    StockQuantity = request.Quantity
                };

                await CreateAsync(variant);

                serviceResponse.ResultCode = ResultCode.Success;
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
        /// Deletes a product variant
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(string variantId)
        {
            if (variantId == null)
                throw new ArgumentNullException(nameof(variantId));

            await _productVariantRepository.DeleteAsync(variantId);
        }

        /// <summary>
        /// Gets product variant with queryable
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<ProductVariant> Get(Expression<Func<ProductVariant, bool>> predicate = null)
        {
            return _productVariantRepository.Get(predicate);
        }

        /// <summary>
        /// Gets a product variant by id
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        public virtual async Task<ProductVariantDto> GetByIdAsync(string variantId)
        {
            if (string.IsNullOrEmpty(variantId))
                throw new ArgumentNullException(nameof(variantId));

            var variant = await _productVariantRepository.GetByIdAsync(variantId);

            return AutoMapperConfiguration.Mapper.Map<ProductVariantDto>(variant);
        }

        /// <summary>
        /// Gets a product by variant id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<List<ProductVariantDto>> GetByProductIdAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                throw new ArgumentNullException(nameof(productId));

            var variantList = await _productVariantRepository.GetListAsync(x => x.ProductId == productId);

            return AutoMapperConfiguration.Mapper.Map<List<ProductVariantDto>>(variantList);
        }

        /// <summary>
        /// Gets product variant lists
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<ProductVariantDto>> GetListAsync()
        {
            var variantList = await _productVariantRepository.GetListAsync();

            return AutoMapperConfiguration.Mapper.Map<List<ProductVariantDto>>(variantList);
        }

        /// <summary>
        /// Updates a product variant
        /// </summary>
        /// <param name="request"></param>
        public virtual async Task<ServiceResponse<object>> UpdateAsync(ProductVariantUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var serviceResponse = new ServiceResponse<object>
            {
                Success = true
            };

            try
            {
                var productVariant = await _productVariantRepository.GetByIdAsync(request.Id);

                if (productVariant == null)
                    throw new ArgumentNullException(nameof(productVariant));

                productVariant.ProductId = request.ProductId;
                productVariant.Sku = request.Sku;
                productVariant.Barcode = request.Barcode;
                productVariant.MinStockQuantity = request.MinStockQuantity;
                productVariant.StockQuantity = request.Quantity;
                productVariant.UpdatedAt = DateTime.UtcNow;

                await UpdateAsync(productVariant);

                serviceResponse.ResultCode = ResultCode.Success;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                _ = _logService.InsertLogAsync(LogLevel.Error, $"ProductVariantService-UpdateAsync Error: model {JsonConvert.SerializeObject(request)}", ex.Message.ToString());
                serviceResponse.Success = false;
                serviceResponse.ResultCode = ResultCode.Exception;
                serviceResponse.Warnings.Add(ex.Message);
                return serviceResponse;
            }
        }

        /// <summary>
        /// Updates a product
        /// </summary>
        /// <param name="productVariant"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(ProductVariant productVariant)
        {
            if (productVariant == null)
                throw new ArgumentNullException(nameof(productVariant));

            await _productVariantRepository.UpdateAsync(productVariant, x => x.Id == productVariant.Id);
        }

        #endregion
    }
}
