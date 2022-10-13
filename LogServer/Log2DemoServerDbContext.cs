using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Log2Server.Models;

#nullable disable

namespace Log2Server
{
    public partial class Log2DemoServerDbContext : DbContext
    {
        public Log2DemoServerDbContext()
        {
        }

        public Log2DemoServerDbContext(DbContextOptions<Log2DemoServerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LsApp> LsApps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LsApp>(entity =>
            {
                entity.Property(e => e.Id).HasComment("ID");

                entity.Property(e => e.AppKey)
                    .IsFixedLength(true)
                    .HasComment("键");

                entity.Property(e => e.AppSecret)
                    .IsFixedLength(true)
                    .HasComment("密钥");

                entity.Property(e => e.CreateAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("创建时间");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
