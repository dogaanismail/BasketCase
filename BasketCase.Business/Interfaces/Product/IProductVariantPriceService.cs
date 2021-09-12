using BasketCase.Core.Domain.Product;
using BasketCase.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketCase.Business.Interfaces.Product
{
    /// <summary>
    /// IProductVariantPriceService interface implementations
    /// </summary>
    public interface IProductVariantPriceService
    {
        /// <summary>
        /// Inserts a variant price
        /// </summary>
        /// <param name="variantPrices"></param>
        Task<ResultModel> CreateAsync(ProductVariantPrice variantPrices);

        /// <summary>
        /// Inserts variant prices by using bulk
        /// </summary>
        /// <param name="variantPrices"></param>
        Task<ResultModel> CreateAsync(List<ProductVariantPrice> variantPrices);

        /// <summary>
        /// Gets a variant price by variant id
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        Task<ProductVariantPrice> GetByIdAsync(string variantId);

        /// <summary>
        /// Gets variant prices by variant id
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        Task<ProductVariantPrice> GetByVariantIdAsync(string variantId);
    }
}
