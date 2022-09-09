using AutoMapper;
using GeekShooping.ProductApi.Data.ValueObjects;
using GeekShooping.ProductApi.Model;

namespace GeekShooping.ProductApi.Config
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
