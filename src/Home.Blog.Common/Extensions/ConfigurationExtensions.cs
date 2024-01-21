namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationExtensions
    {
        public static string? ToConnectionString(this IConfiguration configuration, string name) =>
            configuration?.GetSection("connectionString")[name];
    }
}