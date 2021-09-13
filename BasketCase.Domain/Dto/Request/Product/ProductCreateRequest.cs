namespace BasketCase.Domain.Dto.Request.Product
{
    public class ProductCreateRequest
    {
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public string AlternativeName { get; set; }

        public string Title { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }
    }
}
