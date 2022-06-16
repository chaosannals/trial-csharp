using Microsoft.AspNetCore.Mvc.ApplicationModels;
using SqlSugar;
using HttpServer;
using HttpServer.Middlewares;
using HttpServer.Utilities;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSingleton(op => new SignManager(op.GetRequiredService<SqlSugarScope>()));
//builder.Services.AddSingleton(op => new RecordManager(
//    op.GetRequiredService<SqlSugarScope>(),
//    op.GetRequiredService<ILogger<RecordManager>>()
//));
builder.Services.AddSingleton(op => new WorkQueue(1024));
builder.Services.AddHostedService(op =>
{
    return new WorkService(
        op.GetRequiredService<ILogger<WorkService>>(),
        op.GetRequiredService<WorkQueue>()
    );
});
builder.Services.AddSingleton(op => new LogRecordQueue(1024));
builder.Services.AddHostedService(op =>
{
    return new LogRecordService(
        TimeSpan.FromSeconds(5),
        op.GetRequiredService<ILogger<LogRecordService>>(),
        op.GetRequiredService<LogRecordQueue>(),
        op.GetRequiredService<SqlSugarScope>()
    );
});
builder.Services.AddResponseCompression(op =>
{
    op.EnableForHttps = true;
});


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
