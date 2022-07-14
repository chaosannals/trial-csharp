namespace HttpServer.Utilities;

public class SignToken
{
    public long AppId { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime ExpireAt { get; set; }
}
