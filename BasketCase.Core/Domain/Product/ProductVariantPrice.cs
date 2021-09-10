using BasketCase.Core.Entities;

namespace BasketCase.Core.Domain.Product
{
    public class ProductVariantPrice : BaseEntity
    {
        public string ProductVariantId { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }

        public decimal TaxRate { get; set; }

        public decimal SpecialPrice { get; set; }
    }
}
