using System.Data.SqlClient;

var sqlBuilder = new SqlConnectionStringBuilder
{
    DataSource = "localhost",
    InitialCatalog = "mainDB",
    Password = "Y;9r.5JQ6cwy@)V_",
    UserID = "sqluser"
};


// 通过这个函数，可以查看 SQL SERVER 构建的 链接字符串。
Console.WriteLine(sqlBuilder.ToString());
Console.ReadKey();