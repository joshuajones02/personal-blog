namespace Home.Blog.Mvc.Settings;

public abstract class BaseSettings
{
    public T? Get<T>(string key) =>
        EnvironmentExtensions.GetEnvironmentVariable<T>(key);

    public T? Get<T>(string key, T @default) =>
        EnvironmentExtensions.GetEnvironmentVariable<T>(key, @default);

    public T GetRequired<T>(string key) =>
        EnvironmentExtensions.GetRequiredEnvironmentVariable<T>(key);
}