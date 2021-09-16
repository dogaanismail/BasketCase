namespace BasketCase.Domain.Dto.Response.Product
{
    public class ProductVariantDto
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string Sku { get; set; }

        public string Barcode { get; set; }

        public int MinStockQuantity { get; set; }

        public int StockQuantity { get; set; }
    }
}
