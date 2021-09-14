using BasketCase.Core.Entities;

namespace BasketCase.Core.Domain.Product
{
    public class ProductVariant : BaseEntity
    {
        public string ProductId { get; set; }

        public string Sku { get; set; }

        public string Barcode { get; set; }

        public int MinStockQuantity { get; set; }

        public int StockQuantity { get; set; }
    }
}
