using BasketCase.Core.Caching;
using BasketCase.Core.Domain.Configuration;

namespace BasketCase.Business.Configuration.Common
{
    /// <summary>
    /// Represents default values related to configuration services
    /// </summary>
    public static partial class SystemConfigurationDefaults
    {

        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey SettingsAllAsDictionaryCacheKey => new("Basketcase.setting.all.dictionary.", EntityCacheDefaults<Setting>.Prefix);

        #endregion

        /// <summary>
        /// Gets the path to file that contains app settings
        /// </summary>
        public static string AppConfigsFilePath => "appsettings.json";
    }
}
