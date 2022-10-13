using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LogServer.Models
{
    [Table("ls_app")]
    [Index("AppKey", Name = "APP_KEY_UNIQUE", IsUnique = true)]
    [Index("CreateAt", Name = "CREATE_AT_INDEX")]
    public partial class LsApp
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }
        [Column("app_key")]
        [StringLength(32)]
        public string AppKey { get; set; } = null!;
        [Column("app_secret")]
        [StringLength(64)]
        public string AppSecret { get; set; } = null!;
        [Column("create_at", TypeName = "datetime")]
        public DateTime CreateAt { get; set; }
    }
}
