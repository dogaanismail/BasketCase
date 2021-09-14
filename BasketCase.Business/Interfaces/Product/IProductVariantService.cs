using BasketCase.Core.Domain.Product;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.Product;
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
        /// <param name="productVariant"></param>
        Task CreateAsync(ProductVariant productVariant);

        /// <summary>
        /// Inserts product variants by using bulk
        /// </summary>
        /// <param name="productVariants"></param>
        Task CreateAsync(List<ProductVariant> productVariants);

        /// <summary>
        /// Deletes a product variant
        /// </summary>
        /// <param name="productVariant"></param>
        /// <returns></returns>
        Task DeleteAsync(ProductVariant productVariant);

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

        /// <summary>
        /// Inserts a product variant
        /// </summary>
        /// <param name="request"></param>
        Task<ServiceResponse<object>> CreateAsync(ProductVariantCreateRequest request);

        /// <summary>
        /// Gets product variant lists
        /// </summary>
        /// <returns></returns>
        List<ProductVariant> GetVariants();
    }
}
