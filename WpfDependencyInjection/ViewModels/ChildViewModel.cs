using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;
using WpfLibrary;

namespace WpfDependencyInjection.ViewModels;

public sealed partial class ChildViewModel : ObservableRecipient, IIndexedViewModel
{
    private CompositeDisposable? _disposables;
    private readonly IExternalObject _externalObject;
    private readonly ILogger<ChildViewModel> _logger;

    public PageIndex Index { get; }

    public string Message { get; }

    [ObservableProperty]
    IReadOnlyReactiveProperty<int>? _libCounter;

    [ObservableProperty]
    int _vmCounter;

    public ChildViewModel(
        IPageIndexCounter counter,
        IExternalObject externalObject,
        ILogger<ChildViewModel> logger)
    {
        Index = counter.Index;
        _externalObject = externalObject;
        _logger = logger;
        Message = externalObject.GetData();
    }

    private void Loaded()
    {
        if (_disposables != null && !_disposables.IsDisposed)
            throw new InvalidOperationException($"{GetType().Name} is already loaded.");

        var disposables = new CompositeDisposable();
        System.Reactive.Disposables.Disposable.Create(() => IsActive = false).AddTo(disposables);

        LibCounter = _externalObject.ObserveProperty(static x => x.Counter)
            .ToReadOnlyReactiveProperty().AddTo(disposables);

        _disposables = disposables;
        IsActive = true;
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

    [RelayCommand]
    private void IncrementLibCount()
    {
        _externalObject.IncrementCounter();
    }

    [RelayCommand]
    private void IncrementVmCount()
    {
        _logger.LogTrace("Message.Send<IncrementVmCounterRequestMessage> (Before={Counter})", VmCounter);
        WeakReferenceMessenger.Default.Send<IncrementVmCounterRequestMessage>();
    }

    sealed class IncrementVmCounterRequestMessage : RequestMessage<nint> { }

    protected override void OnActivated()
    {
        Messenger.Register<ChildViewModel, IncrementVmCounterRequestMessage>(this, static (r, _) =>
        {
            r.VmCounter++;
            r._logger.LogTrace("CountUp (Index={Index}, After={Counter})", r.Index, r.VmCounter);
        });
    }
}
