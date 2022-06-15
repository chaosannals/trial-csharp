using SqlSugar;

namespace HttpServer.Models;

[SplitTable(SplitType.Day)]
[SugarTable("log_record_{year}{month}{day}")]
public class MainLogRecord
{
    [SugarColumn(IsPrimaryKey = true, ColumnName ="id")]
    public long Id { get; set; }

    [SugarColumn(ColumnName="app_id", IsNullable = false)]
    public long AppId { get; set; }

    [SugarColumn(ColumnName="content", IsNullable = false, Length = 50000)]
    public string Content { get; set; } = null!;

    /// <summary>
    /// Desc:创建时间
    /// Default:CURRENT_TIMESTAMP
    /// Nullable:False
    /// </summary>  
    [SplitField] // 分表依据字段
    [SugarColumn(ColumnName ="create_at", ColumnDescription = "创建时间")]
    public DateTime CreateAt { get; set; }
}
