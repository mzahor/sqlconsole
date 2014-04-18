using System.Configuration;
using SQLConsole.BizLogic.Configuration;
using SqlConsole.BizLogic.Exceptions;

namespace SqlConsole.BizLogic.Configuration
{
    public class UserSettings : IUserSettings
    {
        private readonly System.Configuration.Configuration _configuration;
        private readonly UserSettingsConfigSection _userConfigSection;

        internal UserSettings()
        {
            try
            {
                _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                _userConfigSection =
                    _configuration.GetSection(ConfigurationConstants.UserSettingsSectionName) as
                    UserSettingsConfigSection;

                if (_userConfigSection == null)
                {
                    throw new BizLogicException(ExceptionMessages.ConfigurationSectionNotFound);
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new BizLogicException(ExceptionMessages.ConfigurationError, ex);
            }
        }

        #region IUserSettings Members

        public string ConnectionString
        {
            get { return _userConfigSection.ConnectionString; }
            set
            {
                _userConfigSection.ConnectionString = value;
                _configuration.Save();
            }
        }

        #endregion
    }
}