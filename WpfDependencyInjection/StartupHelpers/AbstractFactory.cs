#if false
using Microsoft.Extensions.DependencyInjection;

namespace WpfDependencyInjection.StartupHelpers;

public interface IAbstractFactory<T>
{
    T Create();
}

internal sealed class AbstractFactory<T> : IAbstractFactory<T>
{
    private readonly Func<T> _factory;

    public AbstractFactory(Func<T> factory)
    {
        _factory = factory;
    }

    public T Create() => _factory();
}

internal static class AbstractFactoryEx
{
    /// <summary>
    /// T を生成する Factory を登録します
    /// </summary>
    internal static void AddTransientFactory<T>(this IServiceCollection services)
        where T : class
    {
        services.AddTransient<T>();
        services.AddSingleton<Func<T>>(static x => () => x.GetService<T>()!);
        services.AddSingleton<IAbstractFactory<T>, AbstractFactory<T>>();
    }
}
#endif
