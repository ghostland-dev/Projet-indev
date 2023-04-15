using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Alimbio.Server.Models.alimbio_master_db;

namespace Alimbio.Server.Data
{
    public partial class alimbio_master_dbContext : DbContext
    {
        public alimbio_master_dbContext()
        {
        }

        public alimbio_master_dbContext(DbContextOptions<alimbio_master_dbContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<Alimbio.Server.Models.alimbio_master_db.Salary> Salaries { get; set; }
    }
}