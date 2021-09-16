using BasketCase.Business.Interfaces.Logging;
using BasketCase.Core;
using BasketCase.Core.Domain.Logging;
using BasketCase.Domain.Enumerations;
using BasketCase.Repository.Generic;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace BasketCase.Business.Services.Logging
{
    /// <summary>
    /// Log service implementations
    /// </summary>
    public partial class LogService : ILogService
    {
        #region Fields
        private readonly IRepository<Log> _logRepository;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor
        public LogService(IRepository<Log> logRepository,
            IWebHelper webHelper)
        {
            _logRepository = logRepository;
            _webHelper = webHelper;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Error logging
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public virtual async Task ErrorAsync(string message, Exception exception = null)
        {
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Error))
                await InsertLogAsync(LogLevel.Error, message, exception?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Information logging
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public virtual async Task InformationAsync(string message, Exception exception = null)
        {
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Information))
                await InsertLogAsync(LogLevel.Information, message, exception?.ToString() ?? string.Empty);
        }

        public virtual async Task<Log> InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "")
        {
            var log = new Log
            {
                Id = ObjectId.GenerateNewId().ToString(),
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                IpAddress = _webHelper.GetCurrentIpAddress(),
                PageUrl = _webHelper.GetThisPageUrl(true),
                ReferrerUrl = _webHelper.GetUrlReferrer(),
            };

            await _logRepository.AddAsync(log);

            return log;
        }

        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel level)
        {
            return level switch
            {
                LogLevel.Debug => false,
                _ => true,
            };
        }

        /// <summary>
        /// Warning logging
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public virtual async Task WarningAsync(string message, Exception exception = null)
        {
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Warning))
                await InsertLogAsync(LogLevel.Warning, message, exception?.ToString() ?? string.Empty);
        }

        #endregion
    }
}
