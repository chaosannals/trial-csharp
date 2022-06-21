using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using HttpServer.Attributters;
using HttpServer.Parameters;
using HttpServer.Models;
using HttpServer.Utilities;
using IdGen;
using System.Threading.Channels;

namespace HttpServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecordController : ControllerBase
{
    private readonly ILogger<RecordController> logger;
    private readonly LogRecordQueue queue;
    private readonly IIdGenerator<long> idGenerator;

    public RecordController(ILogger<RecordController> logger, LogRecordQueue queue, IIdGenerator<long> idGenerator)
    {
        this.logger = logger;
        this.queue = queue;
        this.idGenerator = idGenerator;
    }

    [LogAck]
    [HttpPost]
    public async Task<object> Log([FromBody] LogRecord record)
    {
        //Task.Run(() =>
        //{
        //    Random random = new Random();
        //    var data = new MainLogRecord
        //    {
        //        Content = JsonSerializer.Serialize(record.Record),
        //        CreateAt = DateTime.Now.AddDays(random.NextInt64(-10, 10)),
        //        CreateAt = DateTime.Now,
        //    };
        //    rm.Record("aaa", data);
        //});

        Random random = new Random();
        await queue.EnqueueAsync(new MainLogRecord
        {
            Content = JsonSerializer.Serialize(record.Record),
            // CreateAt = DateTime.Now.AddDays(random.NextInt64(-10, 10)),
            CreateAt = DateTime.Now,
        });

        return new
        {
            Code = 0,
            Message = "写入成功",
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="records"></param>
    /// <returns></returns>
    [Route("many")]
    [LogAck]
    [HttpPost]
    
    public async Task<object> LogMany([FromBody] List<object> records)
    {
        //Task.Run(() =>
        //{
        //    logger.LogInformation("records count: {0}", records.Count);
        //    var data = records.Select(i =>
        //    {
        //        return new MainLogRecord
        //        {
        //            Content = JsonSerializer.Serialize(i),
        //            CreateAt = DateTime.Now,
        //        };
        //    });
        //    rm.RecordMany("bbb", data);
        //});

        await queue.EnqueueAsync(records.Select(i =>
        {
            // Random random = new Random();
            return new MainLogRecord
            {
                Id = idGenerator.CreateId(),
                Content = JsonSerializer.Serialize(i),
                // CreateAt = DateTime.Now.AddDays(random.NextInt64(-10, 10)),
                CreateAt = DateTime.Now,
            };
        }));

        return new
        {
            Code = 0,
            Message = "写入成功",
        };
    }
}
