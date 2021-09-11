using BasketCase.Core.Entities;
using System.Collections.Generic;

namespace BasketCase.Core.Domain.Basket
{
    public class Basket : BaseEntity
    {
        public long CustomerId { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; }
    }
}
