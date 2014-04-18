namespace SQLConsole.BizLogic.DataAccess
{
    public interface ISqlClient
    {
        void RunSql(string sqlCommand);
        T ExecuteScalar<T>(string sqlCommand);
    }
}