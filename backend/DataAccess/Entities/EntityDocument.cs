using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LanguageForge.Api.Entities;

public class EntityDocument : EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; init; }
}
