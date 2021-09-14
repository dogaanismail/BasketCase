﻿using BasketCase.Core.Configuration.Settings;
using BasketCase.Core.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketCase.Business.Interfaces.Configuration
{
    /// <summary>
    /// Setting service interface
    /// </summary>
    public partial interface ISettingService
    {
        /// <summary>
        /// Get setting value by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        Task<T> GetSettingByKeyAsync<T>(string key, T defaultValue = default);

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns></returns>
        IList<Setting> GetAllSettings();

        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> LoadSettingAsync<T>() where T : ISettings, new();

        /// <summary>
        /// Load settings
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ISettings> LoadSettingAsync(Type type);

        /// <summary>
        /// Clear cache
        /// </summary>
        /// <returns></returns>
        Task ClearCacheAsync();
    }
}