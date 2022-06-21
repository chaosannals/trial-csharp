using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using FreeSql;
using HttpServer.Models;

namespace HttpServer.Utilities;

public class LogRecordService : BackgroundService
{
    private readonly ILogger<LogRecordService> logger;
    private readonly LogRecordQueue queue;
    private readonly SqlSugarScope db;
    private readonly IFreeSql fsql;
    private readonly TimeSpan duration;

    public LogRecordService(TimeSpan duration, ILogger<LogRecordService> logger, LogRecordQueue queue, SqlSugarScope db, IFreeSql fsql)
    {
        this.logger=logger;
        this.queue=queue;
        this.db = db;
        this.duration = duration;
        this.fsql=fsql;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"{nameof(WorkService)} is running.");

        var watch = new Stopwatch();
        List<MainLogRecord> records = new List<MainLogRecord>();
        DateTime lastWriteDown = DateTime.Now;
        int batchMaxCount = 1000;
        var repo = fsql.GetRepository<FLogRecord>();
        //var sfg = new SnowflakeGenenator();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                //object? item;
                //while (queue.TryDequeue(out item))
                //{
                //    if (item is MainLogRecord)
                //    {
                //        records.Add((MainLogRecord)item);
                //    }
                //    else if (item is IEnumerable<MainLogRecord>)
                //    {
                //        records.AddRange((item as IEnumerable<MainLogRecord>)!);
                //    }

                //    if (records.Count > batchMaxCount)
                //    {
                //        break;
                //    }
                //}
                while (records.Count < batchMaxCount)
                {
                    object? item = await queue.DequeueAsync(stoppingToken);
                    if (item is null)
                    {
                        break;
                    }
                    records.AddRange((item as IEnumerable<MainLogRecord>)!);
                }


                var now = DateTime.Now;
                //var elapsed = now - lastWriteDown;
                //if (elapsed < duration)
                //{
                //    await Task.Delay(duration - elapsed);
                //}
                lastWriteDown = now;

                if (records.Count > 0)
                {
                    watch.Restart();
                    // long inc = now.Ticks;
                    var frecords = records.Select(i =>
                    {
                        return new FLogRecord
                        {
                            //Id = inc++,
                            //Id = sfg.NextId(),
                            Id=i.Id,
                            Content = i.Content,
                            CreateAt = i.CreateAt,
                        };
                    }).ToList();
                    var dtag = now.ToString("yyyyMMdd");
                    repo.AsTable(n => $"{n}_{dtag}");
                    await repo.InsertAsync(frecords);
                    //fsql.Insert(frecords).AsTable(n => $"{n}_{dtag}");
                    //await db.Insertable(records)
                    //    .SplitTable()
                    //    .ExecuteReturnSnowflakeIdListAsync();
                    //await db.Insertable(records)
                    //    .SplitTable()
                    //    .ExecuteCommandAsync();
                    watch.Stop();
                    logger.LogInformation("log write down {0} use {1}ms", records.Count, watch.Elapsed.TotalMilliseconds);
                    records.Clear();
                }

                //await Task.Yield();
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
