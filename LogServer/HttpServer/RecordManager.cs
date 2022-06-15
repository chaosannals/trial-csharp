using System.Threading;
using System.Diagnostics;
using SqlSugar;
using HttpServer.Models;

namespace HttpServer;

/// <summary>
/// 
/// </summary>
public class RecordManager
{
    private SqlSugarScope db;
    private RecordTask records;
    private RecordTask tasks;
    private DateTime last;
    private TimeSpan duration;
    private Thread worker;
    private readonly ILogger<RecordManager> logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public RecordManager(SqlSugarScope db, ILogger<RecordManager> logger)
    {
        this.db=db;
        records=new RecordTask(logger);
        tasks = new RecordTask(logger);
        last = DateTime.Now;
        duration = TimeSpan.FromSeconds(2);
        worker = new Thread(() =>
        {
            var watch = new Stopwatch();
            var count = 0;
            watch.Start();
            while (true)
            {
                try
                {
                    watch.Restart();
                    lock (records)
                    {
                        if (records.Count > 0)
                        {
                            last = DateTime.Now;
                            records.Switch(tasks);
                        }
                    }
                    count = tasks.Count;
                    if (tasks.Count > 0)
                    {
                        tasks.WriteDown(db);
                        logger.LogInformation("write down final");
                    }
                    watch.Stop();
                    logger.LogInformation("down: {0}ms {1}", watch.ElapsedMilliseconds, count);
                }
                catch (Exception e)
                {
                    logger.LogError("write down: {0}", e);
                }
                Thread.Sleep(duration);
            }
        });
        worker.Start();
        logger.LogInformation("Start Worker.");
        this.logger=logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="record"></param>
    public void Record(string key, MainLogRecord record)
    {
        lock (records)
        {
            records.Record(key, record);
        }
    }

    public void RecordMany(string key, IEnumerable<MainLogRecord> rows)
    {
        lock (records)
        {
            records.RecordMany(key, rows);
        }
    }
}
