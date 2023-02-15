using Lib.AspNetCore.ServerSentEvents;
using System.Security.Cryptography;

namespace MvcDemo.Services;

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
        // return;
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var clients = service.GetClients();
                if (clients.Any())
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
                    await Task.Delay(1000);
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
    //            if (clients.Any())
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
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogInformation("error: {} {}", ex.Message, ex.StackTrace);
    //    }
    //}

    //public async Task StopAsync(CancellationToken cancellationToken)
    //{
    //    await Task.CompletedTask;
    //}
}
