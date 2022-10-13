using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Log2Server.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ls_app",
                columns: table => new
                {
                    id = table.Column<int>(type: "int unsigned", nullable: false, comment: "ID")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    app_key = table.Column<string>(type: "char(32)", fixedLength: true, maxLength: 32, nullable: false, comment: "键"),
                    app_secret = table.Column<string>(type: "char(64)", fixedLength: true, maxLength: 64, nullable: false, comment: "密钥"),
                    create_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ls_app", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "APP_KEY_UNIQUE",
                table: "ls_app",
                column: "app_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CREATE_AT_INDEX",
                table: "ls_app",
                column: "create_at");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ls_app");
        }
    }
}
