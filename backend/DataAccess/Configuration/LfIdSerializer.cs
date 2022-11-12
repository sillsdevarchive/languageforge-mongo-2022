using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using LanguageForge.Api.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace LanguageForge.Api.Configuration;

public class LfIdSerializerProvider : JsonConverterFactory, IBsonSerializationProvider
{
    public static LfIdSerializerProvider Instance { get; } = new LfIdSerializerProvider();

    private LfIdSerializerProvider()
    {
    }

    public IBsonSerializer? GetSerializer(Type type)
    {
        if (type.IsAssignableTo(typeof(LfId)))
        {
            return new LfIdSerializer(type);
        }

        return null;
    }


    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableTo(typeof(LfId));
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new LfIdSerializer(typeToConvert);
    }
}

public class LfIdSerializer : JsonConverter<LfId>, IBsonSerializer
{
    public LfIdSerializer(Type type)
    {
        ValueType = type;
    }

    /// <summary>
    /// read bson into LfId
    /// </summary>
    /// <param name="context"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="SerializationException"></exception>
    public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var id = context.Reader.ReadObjectId();
        if (id == ObjectId.Empty)
        {
            throw new SerializationException("LfId can not be null during bson deserialization");
        }

        return LfId.FromDb(id.ToString(), args.NominalType);
    }

    /// <summary>
    /// write bson from LfId
    /// </summary>
    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        if (value is LfId id)
        {
            context.Writer.WriteObjectId(ObjectId.Parse(id.GetIdForDb()));
        }
    }

    public Type ValueType { get; }


    /// <summary>
    /// read json into LfId
    /// </summary>
    public override LfId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var idString = reader.GetString();
        if (idString == null)
        {
            throw new NullReferenceException("LfId can not be null during json deserialization");
        }

        return LfId.FromJson(idString, typeToConvert);
    }

    /// <summary>
    /// write json from LfId
    /// </summary>
    public override void Write(Utf8JsonWriter writer, LfId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.GetIdForJson());
    }
}
