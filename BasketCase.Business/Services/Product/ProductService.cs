using BasketCase.Business.Interfaces.Product;
using BasketCase.Core.Caching;
using BasketCase.Core.Events;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.Product;
using BasketCase.Repository.Generic;
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

        #endregion

        #region Ctor
        public ProductService(IRepository<ProductEntity> productRepository,
            IEventPublisher eventPublisher,
            IStaticCacheManager staticCacheManager)
        {
            _productRepository = productRepository;
            _eventPublisher = eventPublisher;
            _staticCacheManager = staticCacheManager;
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
                    Name = request.Name,
                    ShortDescription = request.ShortDescription,
                    FullDescription = request.AlternativeName,
                    Title = request.Title
                };

                await CreateAsync(product);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.ResultCode = ResultCode.Exception;
                serviceResponse.Warnings.Add(ex.Message);
                return serviceResponse;
            }
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

        #endregion
    }
}
