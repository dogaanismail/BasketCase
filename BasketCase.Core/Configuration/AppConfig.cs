using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BasketCase.Core.Configuration
{
    /// <summary>
    /// Represents the app configs
    /// </summary>
    public partial class AppConfig
    {
        #region Properties

        /// <summary>
        /// Gets or sets cache configuration parameters
        /// </summary>
        public CacheConfig CacheConfig { get; set; } = new CacheConfig();

        /// <summary>
        /// Gets or sets distributed cache configuration parameters
        /// </summary>
        public DistributedCacheConfig DistributedCacheConfig { get; set; } = new DistributedCacheConfig();

        /// <summary>
        /// Gets or sets distributed cache configuration parameters
        /// </summary>
        public MongoDbConfig MongoDbConfig { get; set; } = new MongoDbConfig();

        /// <summary>
        /// Gets or sets additional configuration parameters
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        #endregion
    }
}
