using BasketCase.Core.Domain.Product;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.Product;
using BasketCase.Domain.Dto.Response.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        /// Inserts a product variant
        /// </summary>
        /// <param name="request"></param>
        Task<ServiceResponse<object>> CreateAsync(ProductVariantCreateRequest request);

        /// <summary>
        /// Updates a product variant
        /// </summary>
        /// <param name="request"></param>
        Task<ServiceResponse<object>> UpdateAsync(ProductVariantUpdateRequest request);

       /// <summary>
       /// Updates a variant
       /// </summary>
       /// <param name="productVariant"></param>
       /// <returns></returns>
        Task UpdateAsync(ProductVariant productVariant);

        /// <summary>
        /// Deletes a product variant
        /// </summary>
        /// <param name="productVariant"></param>
        /// <returns></returns>
        Task DeleteAsync(ProductVariant productVariant);

        /// <summary>
        /// Deletes a product variant
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        Task DeleteAsync(string variantId);

        /// <summary>
        /// Gets a product variant by id
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        Task<ProductVariantDto> GetByIdAsync(string variantId);

        /// <summary>
        /// Gets a product variant by id
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        Task<List<ProductVariantDto>> GetByProductIdAsync(string productId);
     
        /// <summary>
        /// Gets product variant lists
        /// </summary>
        /// <returns></returns>
        Task<List<ProductVariantDto>> GetListAsync();

        /// <summary>
        /// Gets product variant with queryable
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<ProductVariant> Get(Expression<Func<ProductVariant, bool>> predicate = null);
    }
}
