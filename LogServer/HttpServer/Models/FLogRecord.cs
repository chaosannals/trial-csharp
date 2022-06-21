using FreeSql.DataAnnotations;
using FreeSql.DatabaseModel;

namespace HttpServer.Models;

[Table(Name ="log_record")]
public class FLogRecord
{
    [Column(IsPrimary =true, IsIdentity = false)]
    public long Id { get; set; }

    public long AppId { get; set; }

    [Column(DbType = "TEXT")]
    public string Content { get; set; } = null!;

    public DateTime CreateAt { get; set; }
}
