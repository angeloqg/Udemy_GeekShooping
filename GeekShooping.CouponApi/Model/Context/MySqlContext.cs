using Microsoft.EntityFrameworkCore;

namespace GeekShooping.CouponApi.Model.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
    }
}
