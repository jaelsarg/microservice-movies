using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Shared.Messages;
public class SinglePropertyValueObjectsJsonConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsClass || typeToConvert.IsAbstract)
            return false;

        if (!typeof(ValueObject).IsAssignableFrom(typeToConvert))
            return false;

        var properties = typeToConvert
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToArray();

        if (properties.Length != 1)
            return false;

        var propType = properties[0].PropertyType;
        bool supportedProp = propType == typeof(string) || propType == typeof(int) || propType == typeof(long);
        if (!supportedProp)
            return false;

        // Accept factories/ctors that take either the property type or string
        return HasCreateMethod(typeToConvert, propType)
            || HasFromMethod(typeToConvert, propType)
            || HasConstructor(typeToConvert, propType)
            || HasCreateMethod(typeToConvert, typeof(string))
            || HasFromMethod(typeToConvert, typeof(string))
            || HasConstructor(typeToConvert, typeof(string));
    }

    private static bool HasCreateMethod(Type type, Type param)
    {
        return type.GetMethod(
            "Create",
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            types: new[] { param },
            modifiers: null
        ) is not null;
    }

    private static bool HasFromMethod(Type type, Type param)
    {
        return type.GetMethod(
            "From",
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            types: new[] { param },
            modifiers: null
        ) is not null;
    }

    private static bool HasConstructor(Type type, Type param)
    {
        return type.GetConstructor(new[] { param }) is not null;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(SinglePropertyValueObjectConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }

    private sealed class SinglePropertyValueObjectConverter<TValueObject> : JsonConverter<TValueObject>
        where TValueObject : ValueObject
    {
        private static readonly PropertyInfo ValueProperty = GetValueProperty();
        private static readonly Func<object?, TValueObject> FromValue = CreateFromValue();

        public override TValueObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return default;

            var propType = ValueProperty.PropertyType;

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (propType == typeof(int))
                    return FromValue(reader.GetInt32());

                if (propType == typeof(long))
                    return FromValue(reader.GetInt64());

                // fallback: read as string
                var s = reader.GetDouble().ToString(CultureInfo.InvariantCulture);
                return FromValue(s);
            }

            // treat strings and other tokens as string
            var text = reader.GetString() ?? string.Empty;
            return FromValue(text);
        }

        public override void Write(Utf8JsonWriter writer, TValueObject value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            object? propertyValue = ValueProperty.GetValue(value);
            if (propertyValue is null)
            {
                writer.WriteNullValue();
                return;
            }

            var propType = ValueProperty.PropertyType;

            if (propType == typeof(string))
            {
                var text = Convert.ToString(propertyValue, CultureInfo.InvariantCulture) ?? string.Empty;
                writer.WriteStringValue(text);
                return;
            }

            if (propType == typeof(int))
            {
                writer.WriteNumberValue(Convert.ToInt32(propertyValue, CultureInfo.InvariantCulture));
                return;
            }

            if (propType == typeof(long))
            {
                writer.WriteNumberValue(Convert.ToInt64(propertyValue, CultureInfo.InvariantCulture));
                return;
            }

            // fallback to string
            var fallback = Convert.ToString(propertyValue, CultureInfo.InvariantCulture) ?? string.Empty;
            writer.WriteStringValue(fallback);
        }

        private static PropertyInfo GetValueProperty()
        {
            var properties = typeof(TValueObject)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .ToArray();

            if (properties.Length != 1)
                throw new InvalidOperationException($"{typeof(TValueObject).Name} must have exactly one readable property.");

            return properties[0];
        }

        private static Func<object?, TValueObject> CreateFromValue()
        {
            var paramType = ValueProperty.PropertyType;

            // Exact-type factories / ctor
            var createMethod = typeof(TValueObject).GetMethod("Create", BindingFlags.Public | BindingFlags.Static, null, new[] { paramType }, null);
            if (createMethod != null)
                return value => (TValueObject)createMethod.Invoke(null, new[] { ConvertTo(value, paramType) })!;

            var fromMethod = typeof(TValueObject).GetMethod("From", BindingFlags.Public | BindingFlags.Static, null, new[] { paramType }, null);
            if (fromMethod != null)
                return value => (TValueObject)fromMethod.Invoke(null, new[] { ConvertTo(value, paramType) })!;

            var ctor = typeof(TValueObject).GetConstructor(new[] { paramType });
            if (ctor != null)
                return value => (TValueObject)ctor.Invoke(new[] { ConvertTo(value, paramType) })!;

            // Try string-based factories/ctor (value object can parse)
            var createFromString = typeof(TValueObject).GetMethod("Create", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
            if (createFromString != null)
                return value => (TValueObject)createFromString.Invoke(null, new[] { Convert.ToString(value, CultureInfo.InvariantCulture) })!;

            var fromFromString = typeof(TValueObject).GetMethod("From", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
            if (fromFromString != null)
                return value => (TValueObject)fromFromString.Invoke(null, new[] { Convert.ToString(value, CultureInfo.InvariantCulture) })!;

            var ctorString = typeof(TValueObject).GetConstructor(new[] { typeof(string) });
            if (ctorString != null)
                return value => (TValueObject)ctorString.Invoke(new object?[] { Convert.ToString(value, CultureInfo.InvariantCulture) })!;

            throw new InvalidOperationException(
                $"{typeof(TValueObject).Name} must expose a public static Create({paramType.Name}) or From({paramType.Name}) method, or a public {paramType.Name} constructor (or accept string)."
            );
        }

        private static object? ConvertTo(object? value, Type targetType)
        {
            if (value == null)
                return null;

            if (targetType == typeof(string))
                return Convert.ToString(value, CultureInfo.InvariantCulture);

            // If value is already of target type, return it
            if (targetType.IsInstanceOfType(value))
                return value;

            return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
        }
    }
}