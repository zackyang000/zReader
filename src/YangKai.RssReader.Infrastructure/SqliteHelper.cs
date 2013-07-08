using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace YangKai.RssReader.Infrastructure
{
    public class SqliteHelper
    {
        private const string ConnStr = "data source=|DataDirectory|C:\\Dropbox\\db.dll";

        public static object ExecuteScalar(string sql, SQLiteParameter[] param = null)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnStr))
            {
                conn.ConnectionString = ConnStr;
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                if (param != null) cmd.Parameters.AddRange(param);
                return cmd.ExecuteScalar();
            }
        }

        public static object ExecuteScalar(string sql, SQLiteParameter param)
        {
            return ExecuteScalar(sql, new[] {param});
        }

        public static void ExecuteNonQuery(string sql, SQLiteParameter[] param = null)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnStr))
            {
                conn.ConnectionString = ConnStr;
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                if (param != null) cmd.Parameters.AddRange(param);
                cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteNonQuery(string sql, SQLiteParameter param)
        {
           return ExecuteScalar(sql, new[] {param});
        }

        public static DataSet ExecuteDataSet(string sql, SQLiteParameter[] param = null)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnStr))
            {
                conn.Open();
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                if (param != null) cmd.Parameters.AddRange(param);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                return ds;
            }
        }

        public static DataSet ExecuteDataSet(string sql, SQLiteParameter param)
        {
            return ExecuteDataSet(sql, new[] { param });
        }
    }
}
