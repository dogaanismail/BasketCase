using BasketCase.Core.Entities;

namespace BasketCase.Core.Domain.Basket
{
    public class BasketItem : BaseEntity
    {
        public long BasketId { get; set; }
        public long VariantId { get; set; }
        public long VariantPriceId { get; set; }
        public int Quantity { get; set; }
    }
}
