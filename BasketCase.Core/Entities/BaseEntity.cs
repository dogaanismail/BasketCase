using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BasketCase.Core.Entities
{
    /// <summary>
    /// BaseEntity abstract class implementations
    /// </summary>
    public abstract class BaseEntity : IEntity<string>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        [BsonElement(Order = 0)]
        public string Id { get; } = ObjectId.GenerateNewId().ToString();

        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement(Order = 101)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
