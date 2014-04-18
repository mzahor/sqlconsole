using System.Configuration;

namespace SQLConsole.BizLogic.Configuration
{
    public class UserSettingsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty(ConfigurationConstants.ConnectionString, IsDefaultCollection = false, DefaultValue = null, IsRequired = false)]
        public string ConnectionString
        {
            get { return (string) this[ConfigurationConstants.ConnectionString]; }
            set { this[ConfigurationConstants.ConnectionString] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        #region Nested type: ConfigurationConstants

        #endregion
    }

    internal static class ConfigurationConstants
    {
        public const string ConnectionString = "ConnectionString";
        public const string UserSettingsSectionName = "SQLConsoleUserSettings";
    }
}