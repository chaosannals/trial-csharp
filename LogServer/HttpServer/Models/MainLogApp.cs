using SqlSugar;

namespace HttpServer.Models;

[SugarTable("log_app")]
public class MainLogApp
{
    [SugarColumn(IsPrimaryKey = true, ColumnName ="id")]
    public long Id { get; set; }

    [SugarColumn(ColumnName = "app_key", IsNullable = false)]
    public string AppKey { get; set; } = null!;

    [SugarColumn(ColumnName = "app_secret", IsNullable = false)]
    public string AppSecret { get; set; } = null!;

    /// <summary>
    /// Desc:创建时间
    /// Default:CURRENT_TIMESTAMP
    /// Nullable:False
    /// </summary>  
    [SugarColumn(ColumnName = "create_at")]
    public DateTime CreateAt { get; set; }
}
