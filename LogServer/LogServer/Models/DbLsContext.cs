using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LogServer.Models
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

        public virtual DbSet<LsApp> LsApps { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<LsApp>(entity =>
            {
                entity.Property(e => e.AppKey).IsFixedLength();

                entity.Property(e => e.AppSecret).IsFixedLength();

                entity.Property(e => e.CreateAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
