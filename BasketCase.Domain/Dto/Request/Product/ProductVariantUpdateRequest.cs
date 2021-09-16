namespace BasketCase.Domain.Dto.Request.Product
{
    public class ProductVariantUpdateRequest
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string Sku { get; set; }

        public string Barcode { get; set; }

        public int MinStockQuantity { get; set; }

        public int Quantity { get; set; }
    }
}
