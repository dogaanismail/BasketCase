using AutoMapper;
using BasketCase.Core.Domain.Product;
using BasketCase.Core.Infrastructure.Mapper;
using BasketCase.Domain.Dto.Response.Product;

namespace BasketCase.Business.MapperConfig
{
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor
        public MapperConfiguration()
        {
            CreateProductMaps();

            CreateProductVariantMaps();
        }

        #endregion

        #region Utilities
        protected virtual void CreateProductMaps()
        {
            CreateMap<Product, ProductDto>();
        }

        protected virtual void CreateProductVariantMaps()
        {
            CreateMap<ProductVariant, ProductVariantDto>();
        }
        #endregion

        public int Order => 0;
    }
}
