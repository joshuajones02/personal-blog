namespace Home.Blog;

using System;
using System.ComponentModel;

public static class EnvironmentExtensions
{
    public static T GetEnvironmentVariable<T>(string key) =>
        InternalGetEnvironmentVariable<T>(key, default);

    public static T GetEnvironmentVariable<T>(string key, T @default) =>
        InternalGetEnvironmentVariable(key, @default);

    public static T GetRequiredEnvironmentVariable<T>(string key) =>
        InternalGetEnvironmentVariable<T>(key, default, true);

    public static T GetRequiredEnvironmentVariable<T>(string key, T @default) =>
        InternalGetEnvironmentVariable(key, @default, true);

    private static T InternalGetEnvironmentVariable<T>(string key, T @default, bool isRequired = false)
    {
        if (typeof(T).IsClass && typeof(T) != typeof(string))
            throw new InvalidOperationException($"Only value types allowed. \"Type Not Allowed\"={typeof(T).Name}");

        var value = Environment.GetEnvironmentVariable(key);

        if (!string.IsNullOrEmpty(value))
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
        if (isRequired)
            throw new ArgumentNullException(nameof(key),
                $"EnvironmentVariable \"{key}\" was not set and was marked as required.");

        return @default;
    }
}