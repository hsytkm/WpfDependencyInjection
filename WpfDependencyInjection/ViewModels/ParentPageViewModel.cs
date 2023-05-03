using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

namespace WpfDependencyInjection.ViewModels;

public sealed partial class ParentPageViewModel : ObservableObject, IIndexedViewModel
{
    private CompositeDisposable? _disposables;
    private readonly ILogger<ParentPageViewModel> _logger;

    public PageIndex Index { get; }

    [ObservableProperty]
    int _counter;

    public ParentPageViewModel(
        IPageIndexCounter counter,
        ILogger<ParentPageViewModel> logger)
    {
        Index = counter.Index;
        _logger = logger;
    }

    private void Loaded()
    {
        if (_disposables != null && !_disposables.IsDisposed)
            throw new InvalidOperationException($"{GetType().Name} is already loaded.");

        var disposables = new CompositeDisposable();

        Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(250))
            .Subscribe(_ => Counter++).AddTo(disposables);

        _disposables = disposables;
    }

    private void Unloaded()
    {
        _disposables?.Dispose();
        _disposables = null;
    }

    public void ToggleActivation(bool toActive)
    {
        if (toActive)
        {
            _logger.LogTrace("Loaded Index={Index}", Index.Value);
            Loaded();
        }
        else
        {
            _logger.LogTrace("Unloaded Index={Index}", Index.Value);
            Unloaded();
        }
    }
}
