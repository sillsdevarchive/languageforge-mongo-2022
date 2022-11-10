using System.Linq.Expressions;
using MongoDB.Driver;

namespace LanguageForge.Api.Extensions;

public static class MongoCollectionExtensions
{
    public static Task<TProjection?> Update<TDocument, TProjection>(this IMongoCollection<TDocument> collection,
        string id,
        UpdateDefinition<TDocument> update,
        Expression<Func<TDocument, TProjection>> projection
    )
    {
        return collection.FindOneAndUpdateAsync(
            Builders<TDocument>.Filter.Eq("Id", id),
            update,
            new FindOneAndUpdateOptions<TDocument, TProjection>
            {
                Projection = Builders<TDocument>.Projection.Expression(projection),
                ReturnDocument = ReturnDocument.After
            }
        )!;
    }

    public static Task<TProjection?> Update<TDocument, TProjection>(this IMongoCollection<TDocument> collection,
        string id,
        UpdateDefinition<TDocument> update,
        ProjectionDefinition<TDocument, TProjection> projection
    )
    {
        return collection.FindOneAndUpdateAsync(
            Builders<TDocument>.Filter.Eq("Id", id),
            update,
            new FindOneAndUpdateOptions<TDocument, TProjection>
            {
                Projection = projection, ReturnDocument = ReturnDocument.After
            }
        )!;
    }

    public static Task<TDocument?> Update<TDocument>(this IMongoCollection<TDocument> collection,
        string id,
        UpdateDefinition<TDocument> update
    )
    {
        return collection.FindOneAndUpdateAsync(
            Builders<TDocument>.Filter.Eq("Id", id),
            update,
            new FindOneAndUpdateOptions<TDocument, TDocument> { ReturnDocument = ReturnDocument.After }
        )!;
    }
}
