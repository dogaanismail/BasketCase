using BasketCase.Core.Domain.Logging;
using BasketCase.Domain.Enumerations;
using System;
using System.Threading.Tasks;

namespace BasketCase.Business.Interfaces.Logging
{
    /// <summary>
    /// ILogService interface implementations
    /// </summary>
    public partial interface ILogService
    {
        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level">Log level</param>
        /// <returns>Result</returns>
        bool IsEnabled(LogLevel level);

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="shortMessage"></param>
        /// <param name="fullMessage"></param>
        /// <returns></returns>
        Task<Log> InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "");

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        Task InformationAsync(string message, Exception exception = null);

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        Task WarningAsync(string message, Exception exception = null);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        Task ErrorAsync(string message, Exception exception = null);
    }
}