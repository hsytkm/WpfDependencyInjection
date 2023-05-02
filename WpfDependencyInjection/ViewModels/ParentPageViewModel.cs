using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

namespace WpfDependencyInjection.ViewModels;

public sealed partial class ParentPageViewModel : ObservableObject, IIndexedPage
{
    private CompositeDisposable? _disposables;

    public PageIndex Index { get; }

    [ObservableProperty]
    int _counter;

    public ParentPageViewModel(IPageIndexCounter counter)
    {
        Index = counter.Value;
    }

    private void Loaded()
    {
        if (_disposables != null && !_disposables.IsDisposed)
            throw new InvalidOperationException($"{GetType().Name} is already loaded.");

        var disposables = new CompositeDisposable();

        Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(250))
            .Subscribe(_ => Counter++)
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
