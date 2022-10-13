using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Log2Server.Models;

namespace Log2Server;

public class Log2DemoServerDbContextDesignFactory : IDesignTimeDbContextFactory<DbLsContext>
{
    public DbLsContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder();
        var cnf = builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .Add(new JsonConfigurationSource
            {
                Path = "appsettings.json",
                ReloadOnChange = true,
            }).Build();
        var opb = new DbContextOptionsBuilder<DbLsContext>();
        opb.UseMySQL(cnf.GetConnectionString("Main"));
        return new DbLsContext(opb.Options);
    }
}