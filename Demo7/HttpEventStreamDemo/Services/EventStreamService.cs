using Lib.AspNetCore.ServerSentEvents;
using System.Security.Cryptography;

namespace HttpEventStreamDemo.Services;

public class EventStreamService : BackgroundService // IHostedService
{
    private readonly IServerSentEventsService service;
    private readonly ILogger<EventStreamService> logger;

    public EventStreamService(IServerSentEventsService service, ILogger<EventStreamService> logger)
    {
        this.service = service;
        this.logger=logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var clients = service.GetClients();
                if (clients.Any()) // 此处直接取状态，必须 Task.Yield 让出时间，不让会拉满 CPU。
                {
                    var n = RandomNumberGenerator.GetInt32(1, 100);
                    await service.SendEventAsync(
                        new ServerSentEvent
                        {
                            Id = "123",
                            Type = "number",
                            Data = new List<string>
                            {
                                    n.ToString(),
                            }
                        }
                    );
                    await Task.Delay(1000); // 可以处理后要适度发送信息，不然前端收的信息过多，会直接卡死前端。
                }
                await Task.Yield();
            }
        }
        catch (Exception ex)
        {
            logger.LogInformation("error: {} {}", ex.Message, ex.StackTrace);
        }
    }

    //public async Task StartAsync(CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        while (!cancellationToken.IsCancellationRequested)
    //        {
    //            var clients = service.GetClients();
    //            if (clients.Any()) // 此处直接取状态，必须 Task.Yield 让出时间，不让会拉满 CPU。
    //            {
    //                var n = RandomNumberGenerator.GetInt32(1, 100);
    //                await service.SendEventAsync(
    //                    new ServerSentEvent
    //                    {
    //                        Id = "123",
    //                        Type = "number",
    //                        Data = new List<string>
    //                        {
    //                            n.ToString(),
    //                        }
    //                    }
    //                );
    //                await Task.Delay(1000); // 可以处理后要适度发送信息，不然前端收的信息过多，会直接卡死前端。
    //            }
    //            await Task.Yield();
    //        }
    //    }
    //    catch(Exception ex )
    //    {
    //        logger.LogInformation("error: {} {}", ex.Message, ex.StackTrace);
    //    }
    //}

    //public async Task StopAsync(CancellationToken cancellationToken)
    //{
    //    await Task.CompletedTask;
    //}
}
