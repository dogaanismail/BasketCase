using BasketCase.Core.Entities;
using BasketCase.Domain.Enumerations;

namespace BasketCase.Core.Domain.Logging
{
    public class Log : BaseEntity
    {
        public int LogLevelId { get; set; }

        public string ShortMessage { get; set; }

        public string FullMessage { get; set; }

        public string IpAddress { get; set; }

        public string PageUrl { get; set; }

        public string ReferrerUrl { get; set; }

        public LogLevel LogLevel
        {
            get => (LogLevel)LogLevelId;
            set => LogLevelId = (int)value;
        }
    }
}
