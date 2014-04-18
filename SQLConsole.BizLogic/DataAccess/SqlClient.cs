using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using SQLConsole.BizLogic.DataAccess;
using SqlConsole.BizLogic.Configuration;

namespace SqlConsole.BizLogic.DataAccess
{
    public class SqlClient : ISqlClient
    {
        private readonly string _connectionString;

        public SqlClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlClient(IUserSettings userSettings)
        {
            _connectionString = userSettings.ConnectionString;
        }

        #region ISqlClient Members

        public void RunSql(string sqlCommand)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                RunSql(sqlCommand, conn);
            }
        }

        #endregion

        private void RunSql(string sqlCommand, SqlConnection conn)
        {
            var cmd = new SqlCommand
                {
                    CommandText = sqlCommand,
                    CommandType = CommandType.Text,
                    Connection = conn
                };


            conn.Open();

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.Zero))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public T ExecuteScalar<T>(string sqlCommand)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand
                    {
                        CommandText = sqlCommand,
                        CommandType = CommandType.Text,
                        Connection = conn
                    };

                conn.Open();

                return (T) cmd.ExecuteScalar();
            }
        }
    }
}