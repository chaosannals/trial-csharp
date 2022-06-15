namespace HttpServer;

public class SignProof
{
    public long AppId { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public string Secret { get; set; } = null!;
}
