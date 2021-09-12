using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// <param name="post"></param>
        Task CreateAsync(Core.Domain.Product.Product product);

        /// <summary>
        /// Inserts products by using bulk
        /// </summary>
        /// <param name="post"></param>
        Task CreateAsync(List<Core.Domain.Product.Product> products);

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="post"></param>
        Task<ServiceResponse<object>> CreateAsync(ProductCreateRequest request);

        /// <summary>
        /// Gets a product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<Core.Domain.Product.Product> GetByIdAsync(string productId);
    }
}
