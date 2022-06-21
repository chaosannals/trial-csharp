using FreeSql.DataAnnotations;

namespace HttpServer.Models;

[Table(Name = "log_app")]
[Index("ACCOUNT_UNIQUE", "account", IsUnique = true)]
[Index("CREATE_AT_INDEX", "create_at")]
public class FLogApp
{
    [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
    public long Id { get; set; }

    [Column(Name ="account", IsNullable = false)]
    public string Account { get; set; } = null!;

    [Column(Name ="secret", IsNullable = false)]
    public string Secret { get; set; } = null!;

    [Column(Name = "create_at", IsNullable = false)]
    public DateTime CreateAt { get; set; }

    [Column(Name = "remark", IsNullable = true)]
    public string? Remark { get; set; }
}
