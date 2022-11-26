using GrpcServerDemo.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

// Ŀǰ��֧�� Swagger ���������������ʾʹ�� GRPC �ͻ��ˡ�
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ��֧��
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
