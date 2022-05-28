using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Data;
using System.Data.SQLite;
using DynUpdateLibrary.Core.Properties;
using DynUpdateLibrary.Core.Exceptions;

namespace DynUpdateLibrary.Core
{
    public class SQLiteSession : IDisposable
    {
        public string FilePath { get; private set; }
        public SQLiteConnection Connection { get; private set; }
        public SQLiteSession(string path)
        {
            FilePath = path;
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<T> Search<T>(string sql, object args = null)
        {
            using (SQLiteCommand command = NewCommand(sql, args))
            {
                SQLiteDataReader reader = command.ExecuteReader();
                List<T> result = new List<T>();
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();
                while (reader.Read())
                {
                    T one = (T)type.Assembly.CreateInstance(type.FullName);
                    foreach (PropertyInfo p in properties)
                    {
                        var v = reader[p.Name];
                        if (v is DBNull) v = null;
                        p.SetValue(one, v, null);
                    }
                    result.Add(one);
                }
                reader.Close();
                return result;
            }
        }

        public bool Has(string sql, object args = null)
        {
            using (SQLiteCommand command = NewCommand(sql, args))
            {
                SQLiteDataReader reader = command.ExecuteReader();
                bool result = reader.HasRows;
                reader.Close();
                return result;
            }
        }

        public T Find<T>(string sql, object args = null) where T : class
        {
            using (SQLiteCommand command = NewCommand(sql, args))
            {
                SQLiteDataReader reader = command.ExecuteReader();
                try
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    Type type = typeof(T);
                    T result = type.Assembly.CreateInstance(type.FullName) as T;
                    for (int i = 0; i < reader.FieldCount; ++i)
                    {
                        string pn = reader.GetName(i);
                        PropertyInfo p = type.GetProperty(pn);
                        if (p != null)
                        {
                            object v = reader[pn];
                            if (v is DBNull) v = null;
                            p.SetValue(result, v, null);
                        }
                    }
                    return result;
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public int Add<T>(T one, string table = null)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO ");
            builder.Append(table ?? type.Name);
            builder.Append("(");
            List<string> fields = new List<string>();
            List<string> vhs = new List<string>();
            List<SQLiteParameter> vs = new List<SQLiteParameter>();
            foreach (PropertyInfo p in properties)
            {
                var v = p.GetValue(one, null);
                fields.Add(string.Format("[{0}]", p.Name));
                string h = string.Format("@{0}", p.Name);
                vhs.Add(h);
                vs.Add(new SQLiteParameter(h, v));
            }
            builder.Append(string.Join(",", fields.ToArray()));
            builder.Append(")VALUES(");
            builder.Append(string.Join(",", vhs.ToArray()));
            builder.Append(")");
            // builder.ToString().Log();
            return Execute(builder.ToString(), vs.ToArray());
        }

        /// <summary>
        /// 修改。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="one"></param>
        /// <param name="tag"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public int Edit<T>(T one, string tag = "ID", string table = null)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE ");
            builder.Append(table ?? type.Name);
            builder.Append(" SET ");
            List<string> sets = new List<string>();
            List<SQLiteParameter> vs = new List<SQLiteParameter>();
            foreach (PropertyInfo p in properties)
            {
                if (p.Name == tag) continue;
                var field = p.GetValue(one, null);
                string h = string.Format("@{0}", p.Name);
                vs.Add(new SQLiteParameter(h, field));
                sets.Add(string.Format("[{0}]=@{1}", p.Name, p.Name));
            }
            builder.Append(string.Join(",", sets.ToArray()));

            var tagProperty = type.GetProperty(tag);
            if (tagProperty == null)
            {
                throw new InvalidSQLException("标记字段无效");
            }

            var tagValue = tagProperty.GetValue(one, null);
            if (tagValue == null)
            {
                throw new InvalidSQLException("标记数据不可空");
            }

            builder.Append(string.Format(" WHERE [{0}]={1}", tag, tagValue));
            // builder.ToString().Log();
            return Execute(builder.ToString(), vs.ToArray());
        }

        public void EnsureConnection()
        {
            EnsureDll("SQLite.Interop.dll");
            if (Connection == null)
            {
                Connection = new SQLiteConnection(string.Format("Data Source={0}", FilePath));
            }
            switch (Connection.State)
            {
                case ConnectionState.Broken:
                    Connection.Close();
                    Connection.Open();
                    break;
                case ConnectionState.Closed:
                    Connection.Open();
                    break;
            }
        }

        public int Execute(string sql, object args = null)
        {
            using (SQLiteCommand command = NewCommand(sql, args))
            {
                return command.ExecuteNonQuery();
            }
        }

        public SQLiteCommand NewCommand(string sql, object args = null)
        {
            EnsureConnection();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            command.Parameters.AddRange(ParseArgs(args));
            return command;
        }

        public static void EnsureDll(string name)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);
            if (!File.Exists(path))
            {
                string prefix = Path.GetFileNameWithoutExtension(path).Replace('.', '_');
                string tag = IntPtr.Size == 8 ? "x64" : "x86";
                string field = string.Format("{0}_{1}", prefix, tag);
                byte[] data = Resources.ResourceManager.GetObject(field) as byte[];
                File.WriteAllBytes(path, data);
            }
        }

        public static SQLiteParameter[] ParseArgs(object args)
        {
            if (args is null) return new SQLiteParameter[0];
            return args.GetType().GetProperties().Select(i =>
            {
                return new SQLiteParameter()
                {
                    ParameterName = i.Name,
                    Value = i.GetValue(args, null),
                };
            }).ToArray();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
