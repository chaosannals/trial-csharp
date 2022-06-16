using System.Threading.Channels;
using HttpServer.Exceptions;

namespace HttpServer.Utilities;

public class LogRecordQueue
{
    private readonly Channel<object> queue;

    public LogRecordQueue(int capacity)
    {
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

    public async ValueTask<object> DequeueAsync(CancellationToken cancellationToken)
    {
        return await queue.Reader.ReadAsync(cancellationToken);
    }

    public bool TryDequeue(out object? output)
    {
        return queue.Reader.TryRead(out output);
    }
}
