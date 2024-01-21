using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace Microsoft.Extensions.Configuration;

public static class IConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddConfigurationAsEnvironmentVariables(this IConfigurationBuilder builder, string env)
    {
        if (string.IsNullOrEmpty(env))
            throw new ArgumentNullException(nameof(env));

        env = env.ToLowerInvariant();
        AddEnvironmentVariablesFromFile("appsettings.json");
        AddEnvironmentVariablesFromFile($"appsettings.{env}.json");

        return builder;
    }

    private static void AddEnvironmentVariablesFromFile(string fileName)
    {
        var path = Directory.GetCurrentDirectory();
        var file = Path.Combine(path, fileName);

        if (!File.Exists(file))
            throw new FileNotFoundException(fileName);

        var json = File.ReadAllText(file);

        using (var document = JsonDocument.Parse(json))
        {
            document.RootElement.EnumerateJsonAndSetEnvironmentVariables();
        }
    }

    private static void EnumerateJsonAndSetEnvironmentVariables(this JsonElement element, string parentSectionName = null)
    {
        foreach (var property in element.EnumerateObject())
        {
            var value = property.Value;

            if (value.ValueKind == JsonValueKind.Object)
            {
                value.EnumerateJsonAndSetEnvironmentVariables(property.Name);
            }

            if (value.ValueKind >= JsonValueKind.String)
            {
                var propertyValue = value.GetString();

                if (!string.IsNullOrEmpty(propertyValue))
                {
                    var name = !string.IsNullOrEmpty(parentSectionName)
                        ? string.Concat(parentSectionName, ':', property.Name)
                        : property.Name;

                    Environment.SetEnvironmentVariable(name, propertyValue);
                }
            }
        }
    }
}