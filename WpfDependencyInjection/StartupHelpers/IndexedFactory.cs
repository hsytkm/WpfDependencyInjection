using Microsoft.Extensions.DependencyInjection;

namespace WpfDependencyInjection.StartupHelpers;

public interface IIndexedFactory<T>
{
    T Create();
}

internal sealed class IndexedFactory<T> : IIndexedFactory<T>
{
    private readonly PageIndexCounter _counter;
    private readonly Func<T> _factory;

    public IndexedFactory(IPageIndexCounter counter, Func<T> factory)
    {
        // Increment() を実行するため無理やり実体にキャストします
        _counter = (PageIndexCounter)counter;
        _factory = factory;
    }

    public T Create()
    {
        _counter.Increment();   // インスタンス作成前にインクリメントするルールです
        return _factory();
    }
}

internal static class IndexedFactoryEx
{
    /// <summary>
    /// T を生成する Factory を登録します
    /// </summary>
    internal static void AddIndexedFactory<T>(this IServiceCollection services)
        where T : class
    {
        services.AddTransient<T>();
        services.AddSingleton<Func<T>>(static x => () => x.GetService<T>()!);
        services.AddSingleton<IIndexedFactory<T>, IndexedFactory<T>>();
    }
}
