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
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var work = await queue.DequeueAsync(stoppingToken);
                if (work is MainLogRecord)
                {
                    records.Add((MainLogRecord)work);
                }
                else if (work is IEnumerable<MainLogRecord>)
                {
                    records.AddRange((work as IEnumerable<MainLogRecord>)!);
                }

                //var now = DateTime.Now;
                //var elapsed = now - lastWriteDown;
                //lastWriteDown = now;
                //if (elapsed > duration && records.Count > 0)
                //{
                logger.LogInformation("log write down {0}", records.Count);
                db.Insertable(records)
                    .SplitTable()
                    .ExecuteReturnSnowflakeIdList();
                records.Clear();
                //}
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
