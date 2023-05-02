namespace WpfDependencyInjection.Common;

public interface IIndexedObject
{
    PageIndex Index { get; }
}

public interface IIndexedView : IIndexedObject
{ }

public interface IIndexedViewModel : IIndexedObject
{
    void ToggleActivation(bool toActive);
}
