namespace BasketCase.Domain.Dto.Request.ShoppingCart
{
    public class AddToCartRequest
    {
        public string ProductId { get; set; }
        public string ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
