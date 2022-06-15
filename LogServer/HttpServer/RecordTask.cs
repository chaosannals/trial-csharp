using System.Collections.Concurrent;
using SqlSugar;
using HttpServer.Models;

namespace HttpServer;

using RecordQueue = ConcurrentDictionary<string, ConcurrentQueue<MainLogRecord>>; 

public class RecordTask
{
    private RecordQueue records;
    private ILogger<RecordManager> logger;
    public int Count { get; private set; }

    public RecordTask(ILogger<RecordManager> logger)
    {
        this.logger = logger;
        records = new RecordQueue();
        Count = 0;
    }

    public void Switch(RecordTask other)
    {
        var tmpRecords = records;
        var tmpCount = Count;
        records = other.records;
        Count = other.Count;
        other.records = tmpRecords;
        other.Count = tmpCount;
    }

    public void Record(string key, MainLogRecord record)
    {
        if (!records.ContainsKey(key))
        {
            records[key] = new ConcurrentQueue<MainLogRecord>();
        }
        records[key].Enqueue(record);
        Count++;
    }

    public void RecordMany(string key, IEnumerable<MainLogRecord> rows)
    {
        if (!records.ContainsKey(key))
        {
            records[key] = new ConcurrentQueue<MainLogRecord>();
        }
        //records[key].AddRange(rows);
        foreach(var row in rows)
        {
            records[key].Enqueue(row);
        }
        Count += rows.Count();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public void WriteDown(SqlSugarScope db)
    {
        foreach (var pair in records)
        {
            // logger.LogInformation("down {0} => {1}", pair.Key, pair.Value.Count);
            db.Insertable(pair.Value.ToList())
                .SplitTable()
                .ExecuteReturnSnowflakeIdList();
            pair.Value.Clear();
        }
        Count = 0;
    }
}
