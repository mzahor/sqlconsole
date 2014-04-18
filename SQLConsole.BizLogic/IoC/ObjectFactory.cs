using SQLConsole.BizLogic.DataAccess;
using SQLConsole.BizLogic.TaskRunning;
using SQLConsole.BizLogic.Text;
using SqlConsole.BizLogic.Configuration;
using SqlConsole.BizLogic.DataAccess;
using SqlConsole.BizLogic.TaskRunning;

namespace SqlConsole.BizLogic.IoC
{
    public static class ObjectFactory
    {
        public static IUserSettings GetUserSettings()
        {
            return new UserSettings();
        }

        public static ISqlClient GetSqlClient()
        {
            IUserSettings userSettings = GetUserSettings();

            return new SqlClient(userSettings);
        }

        public static ITaskRunner GetTaskRunner()
        {
            IPerformanceCounter performanceCounter = new PerformanceCounter();
            var taskRunner = new TaskRunner(performanceCounter);

            return taskRunner;
        }

        public static ITextUtils GetTextUtils()
        {
            return new TextUtils();
        }
    }
}