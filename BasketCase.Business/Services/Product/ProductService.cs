using BasketCase.Business.Interfaces.Logging;
using BasketCase.Business.Interfaces.Product;
using BasketCase.Core.Caching;
using BasketCase.Core.Events;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.Product;
using BasketCase.Domain.Enumerations;
using BasketCase.Repository.Generic;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductEntity = BasketCase.Core.Domain.Product.Product;

namespace BasketCase.Business.Services.Product
{
    /// <summary>
    /// ProductService service implementations
    /// </summary>
    public class ProductService : IProductService
    {
        #region Fields
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ILogService _logService;

        #endregion

        #region Ctor
        public ProductService(IRepository<ProductEntity> productRepository,
            IEventPublisher eventPublisher,
            IStaticCacheManager staticCacheManager,
            ILogService logService)
        {
            _productRepository = productRepository;
            _eventPublisher = eventPublisher;
            _staticCacheManager = staticCacheManager;
            _logService = logService;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(ProductEntity product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            await _productRepository.AddAsync(product);

            await _eventPublisher.EntityInsertedAsync(product);
        }

        /// <summary>
        /// Inserts products by using bulk
        /// </summary>
        /// <param name="post"></param>
        public virtual async Task CreateAsync(List<ProductEntity> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            await _productRepository.AddRangeAsync(products);

            foreach (var entity in products)
                await _eventPublisher.EntityInsertedAsync(entity);
        }

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ServiceResponse<object>> CreateAsync(ProductCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var serviceResponse = new ServiceResponse<object>
            {
                Success = true
            };

            try
            {
                ProductEntity product = new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = request.Name,
                    ShortDescription = request.ShortDescription,
                    FullDescription = request.AlternativeName,
                    Title = request.Title,
                    OldPrice = request.OldPrice,
                    NewPrice = request.NewPrice
                };

                await CreateAsync(product);

                serviceResponse.ResultCode = ResultCode.Success;
                return serviceResponse;
            }
            catch (Exception ex)
            {
                _ = _logService.InsertLogAsync(LogLevel.Error, $"ProductService-CreateAsync Error: model {JsonConvert.SerializeObject(request)}", ex.Message.ToString());
                serviceResponse.Success = false;
                serviceResponse.ResultCode = ResultCode.Exception;
                serviceResponse.Warnings.Add(ex.Message);
                return serviceResponse;
            }
        }

        public virtual async Task DeleteAsync(ProductEntity product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            await _productRepository.DeleteAsync(product);

            await _eventPublisher.EntityDeletedAsync(product);
        }

        /// <summary>
        /// Deletes a post by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                throw new ArgumentNullException(nameof(productId));

            var product = await GetByIdAsync(productId);

            await _productRepository.DeleteAsync(productId);

            await _eventPublisher.EntityDeletedAsync(product);
        }

        /// <summary>
        /// Gets a product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ProductEntity> GetByIdAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                throw new ArgumentNullException(nameof(productId));

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<ProductEntity>.ByIdCacheKey, productId);

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                return await _productRepository.GetByIdAsync(productId);
            });
        }

        /// <summary>
        /// Gets product list
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<ProductEntity>> GetListAsync()
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<ProductEntity>.AllCacheKey);

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                return await _productRepository.GetListAsync();
            });
        }

        /// <summary>
        /// Updates a product
        /// </summary>
        /// <param name="product"></param>
        public virtual async Task UpdateAsync(ProductEntity product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            await _productRepository.UpdateAsync(product, x => x.Id == product.Id);

            await _eventPublisher.EntityUpdatedAsync(product);
        }

        #endregion
    }
}
