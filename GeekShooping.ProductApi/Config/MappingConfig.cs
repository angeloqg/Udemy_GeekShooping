using AutoMapper;
using GeekShopping.ProductApi.Data.ValueObjects;
using GeekShopping.ProductApi.Model;

namespace GeekShopping.ProductApi.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductVO, Product>().ReverseMap();
                config.CreateMap<ProductCreateVO, ProductVO>();
                config.CreateMap<Product, ProductUpdateVO>();
                config.CreateMap<ProductUpdateVO, ProductVO>();
            });

            return mappingConfig;
        }
    }
}
