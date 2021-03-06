using BasketCase.Core.Configuration;
using BasketCase.Core.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Text;

namespace BasketCase.Business.Configuration.Common
{
    /// <summary>
    /// Represents the app configs helper
    /// </summary>
    public partial class AppConfigsHelper
    {
        #region Methods

        /// <summary>
        /// Saves app configs for a file that is called appSettings
        /// </summary>
        /// <param name="appconfig"></param>
        /// <param name="fileProvider"></param>
        public static void SaveAppSettings(AppConfig appconfig, ISystemFileProvider fileProvider = null)
        {
            Singleton<AppConfig>.Instance = appconfig ?? throw new ArgumentNullException(nameof(appconfig));

            fileProvider ??= CommonHelper.DefaultFileProvider;

            var filePath = fileProvider.MapPath(SystemConfigurationDefaults.AppConfigsFilePath);
            fileProvider.CreateFile(filePath);

            var additionalData = JsonConvert.DeserializeObject<AppConfig>(fileProvider.ReadAllText(filePath, Encoding.UTF8))?.AdditionalData;
            appconfig.AdditionalData = additionalData;

            var text = JsonConvert.SerializeObject(appconfig, Formatting.Indented);
            fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        #endregion
    }
}
