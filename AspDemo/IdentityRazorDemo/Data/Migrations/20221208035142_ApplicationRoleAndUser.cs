using Microsoft.EntityFrameworkCore.Migrations;
using System.Data;

#nullable disable

namespace IdentityRazorDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationRoleAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 这几行是手动添加的，真奇怪，ASP 有识别 ApplicationUser 的能力
            // 只修改 ApplicationUser 不加入 ApplicationRole 的时候使用 AddDefaultIdentiy 时 Add-Migration 可以生成添加的字段。
            // 为了添加 ApplicationRole 改使用 AddIdentity 后却生成的空，不知道是不是没做这部分功能。
            // 不加入程序也正常启动。20221208035142_ApplicationRoleAndUser.Designer 有声明属性，就是数据库没有字段，注册会报错。
            migrationBuilder.AddColumn<string>(
                name: "JobNumber",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true
            );
            migrationBuilder.AddColumn<DataSetDateTime>(
                name: "LeaveAt",
                table: "AspNetUsers",
                nullable: true
            );
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobNumber",
                table: "AspNetUsers"
            );
            migrationBuilder.DropColumn(
                name: "LeaveAt",
                table: "AspNetUsers"
            );
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetRoles"
            );
        }
    }
}
