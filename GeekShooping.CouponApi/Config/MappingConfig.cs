using AutoMapper;

namespace GeekShooping.CouponApi.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {

            });

            return mappingConfig;
        }
    }
}
