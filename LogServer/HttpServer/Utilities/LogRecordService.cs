using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using HttpServer.Models;

namespace HttpServer.Utilities;

public class LogRecordService : BackgroundService
{
    private readonly ILogger<LogRecordService> logger;
    private readonly LogRecordQueue queue;
    private readonly SqlSugarScope db;
    private readonly TimeSpan duration;

    public LogRecordService(TimeSpan duration, ILogger<LogRecordService> logger, LogRecordQueue queue, SqlSugarScope db)
    {
        this.logger=logger;
        this.queue=queue;
        this.db = db;
        this.duration = duration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"{nameof(WorkService)} is running.");

        var watch = new Stopwatch();
        List<MainLogRecord> records = new List<MainLogRecord>();
        DateTime lastWriteDown = DateTime.Now;
        int batchMaxCount = 1000;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                object? item;
                while (queue.TryDequeue(out item))
                {
                    if (item is MainLogRecord)
                    {
                        records.Add((MainLogRecord)item);
                    }
                    else if (item is IEnumerable<MainLogRecord>)
                    {
                        records.AddRange((item as IEnumerable<MainLogRecord>)!);
                    }
                    
                    if (records.Count > batchMaxCount)
                    {
                        break;
                    }
                }
                // logger.LogInformation("lastWriteDown {0}", lastWriteDown);

                var now = DateTime.Now;
                var elapsed = now - lastWriteDown;
                if (elapsed < duration)
                {
                    await Task.Delay(duration - elapsed);
                }
                lastWriteDown = now;

                if (records.Count > 0)
                {
                    watch.Restart();
                    await db.Insertable(records)
                        .SplitTable()
                        .ExecuteReturnSnowflakeIdListAsync();
                    watch.Stop();
                    logger.LogInformation("log write down {0} use {1}ms", records.Count, watch.Elapsed.TotalMilliseconds);
                    records.Clear();
                }

                await Task.Yield();
            }
            catch (OperationCanceledException e)
            {
                logger.LogWarning("OperationCanceledException: {0} {1}", e.Message, e.StackTrace);
            }
            catch (Exception e)
            {
                logger.LogError(e, "in log record queue.");
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"{nameof(WorkService)} is stopping.");
        await base.StopAsync(cancellationToken);
    }
}
