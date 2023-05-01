using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;
using WpfLibrary;

namespace WpfDependencyInjection.ViewModels;

public sealed partial class Child1ViewModel : ObservableObject, IIndexedPage
{
    private CompositeDisposable? _disposables;
    private readonly IExternalObject _externalObject;

    public string Message { get; }

    [ObservableProperty]
    private IReadOnlyReactiveProperty<int>? _counter;

    [ObservableProperty]
    PageIndex _index;

    public Child1ViewModel(IExternalObject externalObject)
    {
        _externalObject = externalObject;
        Message = externalObject.GetData();
    }

    [RelayCommand]
    private void Increment()
    {
        _externalObject.IncrementCounter();
    }

    private void Loaded()
    {
        if (_disposables != null && !_disposables.IsDisposed)
            throw new InvalidOperationException($"{GetType().Name} is already loaded.");

        var disposables = new CompositeDisposable();

        Counter = _externalObject.ObserveProperty(static x => x.Counter)
            .ToReadOnlyReactiveProperty()
            .AddTo(disposables);

        _disposables = disposables;
    }

    private void Unloaded()
    {
        _disposables?.Dispose();
        _disposables = null;
    }

    public void ToggleActivation(bool toActive)
    {
        if (toActive) Loaded();
        else Unloaded();
    }
}
