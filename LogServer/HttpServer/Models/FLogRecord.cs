using FreeSql.DataAnnotations;
using FreeSql.DatabaseModel;

namespace HttpServer.Models;

[Table(Name ="log_record")]
[Index("APP_INDEX", "app_id, create_at")]
[Index("CREATE_AT_INDEX", "create_at")]
public class FLogRecord
{
    [Column(Name="id", IsPrimary =true, IsIdentity = false)]
    public long Id { get; set; }

    [Column(Name ="app_id", IsNullable =false)]
    public long AppId { get; set; }

    [Column(Name = "content", DbType = "TEXT", IsNullable =false)]
    public string Content { get; set; } = null!;

    [Column(Name ="create_at", IsNullable =false)]
    public DateTime CreateAt { get; set; }
}
