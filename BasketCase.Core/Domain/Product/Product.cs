using BasketCase.Core.Entities;
using System.Collections.Generic;

namespace BasketCase.Core.Domain.Product
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public string AlternativeName { get; set; }

        public string Title { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }

        public bool Deleted { get; set; }

        public bool Published { get; set; }

        public virtual ICollection<ProductVariant> Variants { get; set; }
    }
}
