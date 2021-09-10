using System.Runtime.Serialization;

namespace BasketCase.Domain.Enumerations
{
    /// <summary>
    /// Represents distributed cache types enumerations
    /// </summary>
    public enum DistributedCacheType
    {
        [EnumMember(Value = "memory")]
        Memory,
        [EnumMember(Value = "sqlserver")]
        SqlServer,
        [EnumMember(Value = "redis")]
        Redis
    }
}
