using MongoDB.Bson.Serialization.Attributes;

namespace LanguageForge.Api.Entities;

[BsonIgnoreExtraElements(Inherited = true)]
public abstract class EntityBase
{
}
