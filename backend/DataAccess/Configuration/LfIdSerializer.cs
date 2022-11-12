using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using LanguageForge.Api.Entities;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace LanguageForge.Api.Configuration;

public class LfIdSerializerProvider : JsonConverterFactory, IBsonSerializationProvider
{
    public static LfIdSerializerProvider Instance { get; } = new LfIdSerializerProvider();

    private static readonly MethodInfo WrapHelperMethodInfo =
        new Func<LfIdSerializer, IBsonSerializer>(LfIdSerializer.WrapInSpecificType<object>).Method
            .GetGenericMethodDefinition();

    private LfIdSerializerProvider()
    {
    }

    public IBsonSerializer? GetSerializer(Type type)
    {
        if (type.IsAssignableTo(typeof(LfId)))
        {
            var serializer = new LfIdSerializer(type);
            return WrapHelperMethodInfo.MakeGenericMethod(type).Invoke(null, new[] { serializer }) as IBsonSerializer;
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
        var idString = context.Reader.CurrentBsonType switch
        {
            BsonType.String => context.Reader.ReadString(),
            BsonType.ObjectId => context.Reader.ReadObjectId().ToString(),
            _ => throw new SerializationException("LfId can not be created from bson type " +
                                                  context.Reader.CurrentBsonType)
        };
        if (string.IsNullOrEmpty(idString))
        {
            throw new SerializationException("LfId can not be null during bson deserialization");
        }

        return LfId.FromDb(idString, args.NominalType);
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
    public override LfId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

    public static IBsonSerializer<T> WrapInSpecificType<T>(LfIdSerializer serializer)
    {
        return new LfIdSerializerSpecificType<T>(serializer);
    }

    private class LfIdSerializerSpecificType<T> : IBsonSerializer<T>
    {
        private readonly LfIdSerializer _lfIdSerializer;

        public LfIdSerializerSpecificType(LfIdSerializer lfIdSerializer)
        {
            _lfIdSerializer = lfIdSerializer;
        }


        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
        {
            _lfIdSerializer.Serialize(context, args, value);
        }

        public T Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return (T)_lfIdSerializer.Deserialize(context, args);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            _lfIdSerializer.Serialize(context, args, value);
        }

        public Type ValueType => _lfIdSerializer.ValueType;
    }
}
