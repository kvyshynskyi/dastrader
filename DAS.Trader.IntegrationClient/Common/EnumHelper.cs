using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;

namespace DAS.Trader.IntegrationClient.Common;

public static class EnumHelper
{
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<IConvertible, string?>> LocalCache = new();

    public static string? GetDescription<T>(this T enumValue)
        where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum)
            return null;

        var dictionary = LocalCache.GetOrAdd(typeof(T), new ConcurrentDictionary<IConvertible, string?>());

        var result = dictionary.GetOrAdd(enumValue, input =>
        {
            var value = input.ToString(CultureInfo.InvariantCulture);
            var fieldInfo = input.GetType().GetField(value!);

            if (fieldInfo == null) return value;

            var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs?.Any() == true) value = ((DescriptionAttribute)attrs[0]).Description;

            return value;
        });

        return result;
    }
}