namespace BasketCase.Domain.Dto.Response.Product
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public string AlternativeName { get; set; }

        public string Title { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }

        public bool Deleted { get; set; }

        public bool Published { get; set; }
    }
}
