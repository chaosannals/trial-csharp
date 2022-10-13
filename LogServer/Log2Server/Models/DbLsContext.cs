using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Log2Server.Models
{
    public partial class DbLsContext : DbContext
    {
        public DbLsContext()
        {
        }

        public DbLsContext(DbContextOptions<DbLsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LsApp> LsApps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LsApp>(entity =>
            {
                entity.Property(e => e.AppKey).IsFixedLength(true);

                entity.Property(e => e.AppSecret).IsFixedLength(true);

                entity.Property(e => e.CreateAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
