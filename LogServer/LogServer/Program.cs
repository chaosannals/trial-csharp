using LogServer;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var ioc = new ServiceCollection();
var cb = new ConfigurationBuilder();
var cnf = cb.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .Add(new JsonConfigurationSource
    {
        Path = "appsettings.json",
        ReloadOnChange = true,
    }).Build();
ioc.AddSingleton(op => cnf);

ioc.AddDbContext<LogDemoServerDbContext>(opb =>
{
    var connetionString = cnf.GetConnectionString("Main");
    opb.UseMySQL(connetionString);
});

ioc.AddSingleton(op =>
{
    return new LogDemoServer();
});


var provider = ioc.BuildServiceProvider();
var server = provider.GetRequiredService<LogDemoServer>();

server.Serve();
