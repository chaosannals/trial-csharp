using Microsoft.AspNetCore.Mvc.ApplicationModels;
using SqlSugar;
using FreeSql;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using IdGen.DependencyInjection;
using HttpServer;
using HttpServer.Middlewares;
using HttpServer.Utilities;

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information()
//    .WriteTo.File
//    (
//        path: "logs/logserver-.log",
//        rollingInterval: RollingInterval.Day,
//        rollOnFileSizeLimit: true,
//        fileSizeLimitBytes: 2000000,
//        flushToDiskInterval: TimeSpan.FromSeconds(10),
//        outputTemplate: "[{Timestamp:yy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
//        )
//    .WriteTo.Console(restrictedToMinimumLevel:LogEventLevel.Warning)
//    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//builder.Host.UseSerilog();

builder.Logging
    .AddFile(builder.Configuration.GetSection("LoggingFile"))
    .AddConsole();

builder.Services.AddSingleton(op =>
{
    var cs = builder.Configuration.GetConnectionString("Main");
    var db = new FreeSqlBuilder()
        .UseConnectionString(DataType.MySql, cs)
        .UseAutoSyncStructure(true)
        .Build();
    SignManager.Init(db);
    return db;
});

// Add services to the container.
builder.Services.AddMiniProfiler();
builder.Services.AddControllers(op =>
{
    op.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    op.Filters.Add(new ControllerExceptionFilter());
}).ConfigureApiBehaviorOptions(options =>
{
    var builtInFactory = options.InvalidModelStateResponseFactory;

    options.InvalidModelStateResponseFactory = context =>
    {
        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Program>>();

        foreach (var item in context.ModelState)
        {
            logger.LogError("{0} {1}", item.Key, item.Value.Errors[0].ErrorMessage);
        }

        //context.HttpContext.Response.WriteAsJsonAsync();
        
        return builtInFactory(context);
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(op =>
{
    return new SqlSugarScope(new ConnectionConfig()
    {
        ConnectionString = builder.Configuration.GetConnectionString("Main"),
        DbType = DbType.MySql,
        IsAutoCloseConnection = true,
    }, db =>
    {
        db.Aop.OnLogExecuting = (sql, args) =>
        {
            // Console.WriteLine(sql);
        };
    });
});
// v1
builder.Services.AddSingleton(op => new SignManager(op.GetRequiredService<IFreeSql>(), op.GetRequiredService<ILogger<SignManager>>()));
builder.Services.AddSingleton(op => new RecordManager(
    op.GetRequiredService<SqlSugarScope>(),
    op.GetRequiredService<ILogger<RecordManager>>()
));

builder.Services.AddSingleton(op => new WorkQueue(1024));
builder.Services.AddHostedService(op =>
{
    return new WorkService(
        op.GetRequiredService<ILogger<WorkService>>(),
        op.GetRequiredService<WorkQueue>()
    );
});
builder.Services.AddSingleton<LogRecordQueue>();

// v2
builder.Services.AddHostedService(op =>
{
    return new LogRecordService(
        TimeSpan.FromSeconds(2),
        op.GetRequiredService<ILogger<LogRecordService>>(),
        op.GetRequiredService<LogRecordQueue>(),
        op.GetRequiredService<SqlSugarScope>(),
        op.GetRequiredService<IFreeSql>()
    );
});
builder.Services.AddHostedService(op =>
{
    return new SignService(
        op.GetRequiredService<ILogger<SignService>>(),
        op.GetRequiredService<SignManager>()
    );
});

builder.Services.AddResponseCompression(op =>
{
    op.EnableForHttps = true;
});
builder.Services.AddIdGen(100);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseMiniProfiler();
app.UseResponseCompression();

app.UseMiddleware<LogAckMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
