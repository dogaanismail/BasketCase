using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.Product;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductEntity = BasketCase.Core.Domain.Product.Product;

namespace BasketCase.Business.Interfaces.Product
{
    /// <summary>
    /// IProductService interface implementations
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="product"></param>
        Task CreateAsync(ProductEntity product);

        /// <summary>
        /// Inserts products by using bulk
        /// </summary>
        /// <param name="products"></param>
        Task CreateAsync(List<ProductEntity> products);

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="request"></param>
        Task<ServiceResponse<object>> CreateAsync(ProductCreateRequest request);

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="product"></param>
        Task DeleteAsync(ProductEntity product);

        /// <summary>
        /// Deletes a post by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task DeleteAsync(string productId);

        /// <summary>
        /// Updates a product
        /// </summary>
        /// <param name="product"></param>
        Task UpdateAsync(ProductEntity product);

        /// <summary>
        /// Gets a product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductEntity> GetByIdAsync(string productId);

        /// <summary>
        /// Gets product list
        /// </summary>
        /// <returns></returns>
        Task<List<ProductEntity>> GetListAsync();
    }
}
