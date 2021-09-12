using BasketCase.Core.Domain.Product;
using BasketCase.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketCase.Business.Interfaces.Product
{
    /// <summary>
    /// IProductVariantService interface implementations
    /// </summary>
    public interface IProductVariantService
    {
        /// <summary>
        /// Inserts a product variant
        /// </summary>
        /// <param name="product"></param>
        Task<ResultModel> CreateAsync(ProductVariant productVariant);

        /// <summary>
        /// Inserts product variants by using bulk
        /// </summary>
        /// <param name="post"></param>
        Task<ResultModel> CreateAsync(List<ProductVariant> productVariants);

        /// <summary>
        /// Gets a product variant by id
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        Task<ProductVariant> GetByIdAsync(string variantId);

        /// <summary>
        /// Gets a product variant by id
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        Task<ProductVariant> GetByProductIdAsync(string productId);
    }
}
