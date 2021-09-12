using BasketCase.Core.Domain.Product;
using BasketCase.Core.Entities;

namespace BasketCase.Core.Domain.ShoppingCart
{
    public class ShoppingCartItem : BaseEntity
    {
        public string ProductId { get; set; }
        public string VariantId { get; set; }
        public int Quantity { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
}
