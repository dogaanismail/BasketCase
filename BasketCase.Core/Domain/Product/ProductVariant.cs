using BasketCase.Core.Entities;
using System.Collections.Generic;

namespace BasketCase.Core.Domain.Product
{
    public class ProductVariant : BaseEntity
    {
        public string ProductId { get; set; }

        public string Sku { get; set; }

        public string Barcode { get; set; }

        public int MinStockQuantity { get; set; }

        public int Quantity { get; set; }

        public virtual ICollection<ProductVariantPrice> VariantPrices { get; set; }
    }
}
