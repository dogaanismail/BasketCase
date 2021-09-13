using BasketCase.Business.Interfaces.Product;
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
        private readonly IRepository<Core.Domain.Product.Product> _productRepository;

        #endregion

        #region Ctor
        public ProductService(IRepository<Core.Domain.Product.Product> productRepository)
        {
            _productRepository = productRepository;
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

            return await _productRepository.GetByIdAsync(productId);
        }

        #endregion
    }
}
