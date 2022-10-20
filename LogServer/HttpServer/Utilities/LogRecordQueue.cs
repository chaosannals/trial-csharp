using System.Threading.Channels;
using HttpServer.Exceptions;

namespace HttpServer.Utilities;

public class LogRecordQueue
{
    private readonly Channel<object> queue;
    private readonly CancellationTokenSource tokenSource;
    private ILogger<LogRecordQueue> logger;

    public LogRecordQueue(IConfiguration configuration, ILogger<LogRecordQueue> logger)
    {
        int capacity = configuration.GetValue<int>("LogRecordQueue:Capacity");
        this.logger = logger;
        tokenSource = new CancellationTokenSource();
        queue = Channel.CreateBounded<object>(new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
        });
    }

    public async ValueTask EnqueueAsync(object log)
    {
        if (log is null)
        {
            throw new LogException("log is null");
        }
        await queue.Writer.WriteAsync(log);
    }

    public async ValueTask<object?> DequeueAsync(CancellationToken cancellationToken)
    {
        try
        {
            CancellationTokenSource ts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            ts.CancelAfter(TimeSpan.FromSeconds(2));
            //CancellationTokenSource ts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            //ts.Token.Register(() => System.Console.WriteLine("被取消了."));
            return await queue.Reader.ReadAsync(ts.Token);
            //return await queue.Reader.WaitToReadAsync(ts.Token);
        }
        catch (OperationCanceledException e)
        {
            // 通过取消的方式退出，但是这种方式会让程序的退出显示得不好看，有待考虑的方式。
            logger.LogTrace("dequeue cancel {0}", e);
            return null;
        }
    }

    public bool TryDequeue(out object? output)
    {
        return queue.Reader.TryRead(out output);
    }
}
