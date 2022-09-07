﻿using Microsoft.EntityFrameworkCore;

namespace GeekShooping.ProductApi.Model.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(){}
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }
    }
}
