namespace HttpServer.Utilities;

public class SnowflakeGenenator
{
    private static readonly DateTime d1970 = new DateTime(1970, 1, 1);
    private long machineId;
    private long timestamp;
    private long serialNumber;

    public SnowflakeGenenator(long machineId=1)
    {
        this.machineId = machineId << 12;
        serialNumber = 0;
        timestamp = NowTimestamp;
    }

    public long NowTimestamp
    {
        get => (long)(DateTime.Now - d1970).TotalMilliseconds;
    }

    public long NextId()
    {
        var ts = NowTimestamp;
        if (ts == timestamp)
        {
            serialNumber++;
        }
        else
        {
            serialNumber = 0;
        }
        timestamp = ts;
        if (serialNumber >= 4096)
        {
            timestamp++;
            serialNumber = 0;
        }
        return (timestamp << 22) | machineId | serialNumber;
    }

    public long[] NextIdGroup(int count)
    {
        var result = new long[count];
        return result;
    }
}
