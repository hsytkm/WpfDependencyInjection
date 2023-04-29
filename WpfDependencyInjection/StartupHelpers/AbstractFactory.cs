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
