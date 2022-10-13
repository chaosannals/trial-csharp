# Log2Server

```bash
# 需要打开 Nuget 程序包管理控制台
# CodeFirst 更新数据库
Update-Database

# 需要打开 Nuget 程序包管理控制台
# DbFirst 生成模型
Scaffold-DbContext "server=127.0.0.1;port=3306;database=log2demo_server;uid=root;pwd=password;CharSet=utf8mb4;SslMode=none" MySql.EntityFrameworkCore -OutputDir Models -DataAnnotations -NoOnConfiguring -Context DbLsContext -Force

# dotnet 命令，需要安装命令工具
# DbFirst 生成模型
dotnet ef dbcontext scaffold "server=127.0.0.1;port=3306;database=log2demo_server;uid=root;pwd=password;CharSet=utf8mb4;SslMode=none" "MySql.EntityFrameworkCore" -o Models --data-annotations --no-onconfiguring -c DbLsContext -f
```