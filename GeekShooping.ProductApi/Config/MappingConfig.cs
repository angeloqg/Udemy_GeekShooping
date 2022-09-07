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
                config.CreateMap<ProductVO, Product>();
                config.CreateMap<Product, ProductVO>();
            });

            return mappingConfig;
        }
    }
}
