using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Log2Server.Models
{
    [Table("ls_app")]
    [Index(nameof(AppKey), Name = "APP_KEY_UNIQUE", IsUnique = true)]
    [Index(nameof(CreateAt), Name = "CREATE_AT_INDEX")]
    public partial class LsApp
    {
        [Key]
        [Column("id", TypeName = "int unsigned")]
        public int Id { get; set; }
        [Required]
        [Column("app_key")]
        [StringLength(32)]
        public string AppKey { get; set; }
        [Required]
        [Column("app_secret")]
        [StringLength(64)]
        public string AppSecret { get; set; }
        [Column("create_at")]
        public DateTime CreateAt { get; set; }
    }
}
