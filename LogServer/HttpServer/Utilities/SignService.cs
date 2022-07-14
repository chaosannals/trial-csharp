namespace HttpServer.Utilities;

public class SignService : BackgroundService
{
    private readonly ILogger<SignService> logger;
    private readonly SignManager sm;
    private readonly int interval;

    public SignService(ILogger<SignService> logger, SignManager sm, int interval =10000)
    {
        this.logger=logger;
        this.sm=sm;
        this.interval=interval;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var ttc = sm.TokenCount;
            var tcc = sm.ClearTokens();
            var tfc = sm.TokenCount;
            if (tcc > 0)
            {
                logger.LogInformation("tokens: {0} - {1} = {2}", ttc, tcc, tfc);
            }

            var tsc = sm.SignCount;
            var scc = sm.ClearSigns();
            var sfc = sm.SignCount;
            if (scc > 0)
            {
                logger.LogInformation("signs: {0} - {1} = {2}", tsc, scc, sfc);
            }
            await Task.Delay(interval);
        }
    }
}
