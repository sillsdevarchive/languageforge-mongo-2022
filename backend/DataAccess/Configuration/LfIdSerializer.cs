using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using LanguageForge.Api.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace LanguageForge.Api.Configuration;

public class LfIdSerializerProvider : JsonConverterFactory, IBsonSerializationProvider
{
    public static LfIdSerializerProvider Instance { get; } = new LfIdSerializerProvider();

    private static readonly MethodInfo WrapHelperMethodInfo =
        new Func<LfIdSerializer, IBsonSerializer>(LfIdSerializer.WrapInSpecificType<object>).Method
            .GetGenericMethodDefinition();

    private static readonly MethodInfo DictionarySerializerCreatorMethodInfo =
        new Func<IBsonSerializer>(CreateDictionarySerializer<object, object>).Method
            .GetGenericMethodDefinition();

    private LfIdSerializerProvider()
    {
    }

    public IBsonSerializer? GetSerializer(Type type)
    {
        if (IsLfId(type))
        {
            var serializer = new LfIdSerializer(type);
            return WrapHelperMethodInfo.MakeGenericMethod(type).Invoke(null, new[] { serializer }) as IBsonSerializer;
        }
        else if (IsDictionaryOfLfIds(type))
        {
            var lfIdType = type.GenericTypeArguments[0];
            var valueType = type.GenericTypeArguments[1];
            return DictionarySerializerCreatorMethodInfo.MakeGenericMethod(lfIdType, valueType).Invoke(null, null) as IBsonSerializer;
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

    private static bool IsLfId(Type type)
    {
        return type.IsAssignableTo(typeof(LfId));
    }

    private static bool IsDictionaryOfLfIds(Type type)
    {
        return type.GetInterfaces().Any(i => i.IsGenericType &&
            (i.GetGenericTypeDefinition() == typeof(IDictionary<,>)) &&
            IsLfId(i.GetGenericArguments()[0]));
    }

    private static IBsonSerializer CreateDictionarySerializer<TKey, TValue>() where TKey : notnull
    {
        var lfIdSerializer = new LfIdSerializer(typeof(TKey), BsonType.String);
        var typedLfIdSerializer = LfIdSerializer.WrapInSpecificType<TKey>(lfIdSerializer);
        return new DictionaryInterfaceImplementerSerializer<Dictionary<TKey, TValue>>(DictionaryRepresentation.Document,
            typedLfIdSerializer, BsonSerializer.LookupSerializer(typeof(TValue)));
    }
}

public class LfIdSerializer : JsonConverter<LfId>, IBsonSerializer
{
    private readonly BsonType _representation = BsonType.ObjectId;

    public LfIdSerializer(Type type)
    {
        ValueType = type;
    }

    public LfIdSerializer(Type type, BsonType representation) : this(type)
    {
        _representation = representation;
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
            switch (_representation)
            {
                case BsonType.String:
                    context.Writer.WriteString(id.GetIdForDb());
                    break;
                case BsonType.ObjectId:
                    context.Writer.WriteObjectId(ObjectId.Parse(id.GetIdForDb()));
                    break;
                default:
                    throw new SerializationException("LfId can not be serialized to bson type " + _representation);
            }
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
