using System.Threading.Channels;
using HttpServer.Exceptions;

namespace HttpServer.Utilities;

public class WorkQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> queue;

    public WorkQueue(int capacity)
    {
        queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
        });
    }

    public async ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> work)
    {
        if (work is null)
        {
            throw new LogWorkIsNullException(nameof(work));
        }
        await queue.Writer.WriteAsync(work);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await queue.Reader.ReadAsync(cancellationToken);
    }
}
