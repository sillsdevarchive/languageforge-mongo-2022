using MongoDB.Bson.Serialization.Attributes;

namespace LanguageForge.Api.Entities;

public class EntityDocument<T> : EntityBase
{
    [BsonId]
    public required LfId<T> Id { get; init; }
    public required DateTimeOffset DateCreated { get; init; }
}
