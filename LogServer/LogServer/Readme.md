# LogServer

```bash
# 需要打开 Nuget 程序包管理控制台
# [CommentTag] 是标签，比如一开始是 Init
# CodeFirst 修改数据库后保存修改 Migration
Add-Migration [CommentTag]

# 需要打开 Nuget 程序包管理控制台
# CodeFirst 更新数据库
# 此命令如果 schema 不存在，会创建 schema
Update-Database

# 需要打开 Nuget 程序包管理控制台
# DbFirst 生成模型
Scaffold-DbContext "server=127.0.0.1;port=3306;database=logdemo_server;uid=root;pwd=password;CharSet=utf8mb4;SslMode=none" Pomelo.EntityFrameworkCore.Mysql -OutputDir Models -DataAnnotations -NoOnConfiguring -Context DbLsContext -Force

# dotnet 命令，需要安装命令工具
# DbFirst 生成模型
dotnet ef dbcontext scaffold "server=127.0.0.1;port=3306;database=logdemo_server;uid=root;pwd=password;CharSet=utf8mb4;SslMode=none" "Pomelo.EntityFrameworkCore.Mysql" -o Models --data-annotations --no-onconfiguring -c DbLsContext -f
```