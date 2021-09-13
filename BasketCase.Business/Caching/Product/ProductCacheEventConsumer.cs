using System.Threading.Tasks;
using ProductEntity = BasketCase.Core.Domain.Product.Product;

namespace BasketCase.Business.Caching.Product
{
    /// <summary>
    /// Represents a product cache event consumer
    /// </summary>
    public partial class ProductCacheEventConsumer : CacheEventConsumer<ProductEntity>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityEventType"></param>
        /// <returns></returns>
        protected override async Task ClearCacheAsync(ProductEntity entity, EntityEventType entityEventType)
        {
            await base.ClearCacheAsync(entity, entityEventType);
        }
    }
}
