

namespace HttpServer.Utilities;

public class WorkService : BackgroundService
{
    private readonly ILogger<WorkService> logger;
    private readonly WorkQueue queue;

    public WorkService(ILogger<WorkService> logger, WorkQueue queue)
    {
        this.logger=logger;
        this.queue=queue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"{nameof(WorkService)} is running.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var work = await queue.DequeueAsync(stoppingToken);
                await work(stoppingToken);
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception e)
            {
                logger.LogError(e, "");
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"{nameof(WorkService)} is stopping.");
        await base.StopAsync(cancellationToken);
    }
}
