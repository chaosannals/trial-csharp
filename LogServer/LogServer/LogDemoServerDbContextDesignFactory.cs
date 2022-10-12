using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System;

namespace LogServer;

public class LogDemoServerDbContextDesignFactory : IDesignTimeDbContextFactory<LogDemoServerDbContext>
{
    public LogDemoServerDbContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder();
        var cnf = builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .Add(new JsonConfigurationSource
            {
                Path = "appsettings.json",
                ReloadOnChange = true,
            }).Build();
        var opb = new DbContextOptionsBuilder<LogDemoServerDbContext>();
        opb.UseMySQL(cnf.GetConnectionString("Main"));
        return new LogDemoServerDbContext(opb.Options);
    }
}