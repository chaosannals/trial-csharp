﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using LogServer.Models;

namespace LogServer;

public class LogDemoServerDbContextDesignFactory : IDesignTimeDbContextFactory<DbLsContext>
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
        opb.UseMySql(
            cnf.GetConnectionString("Main"),
            new MySqlServerVersion(new Version(8, 0, 26))
        );
        return new DbLsContext(opb.Options);
    }
}