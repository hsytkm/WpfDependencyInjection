using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;
using WpfLibrary;

namespace WpfDependencyInjection.ViewModels;

public sealed partial class ChildViewModel : ObservableRecipient, IIndexedPage
{
    private CompositeDisposable? _disposables;
    private readonly IExternalObject _externalObject;

    public string Message { get; }

    [ObservableProperty]
    IReadOnlyReactiveProperty<int>? _libCounter;

    [ObservableProperty]
    int _vmCounter;

    [ObservableProperty]
    PageIndex _index;

    public ChildViewModel(IExternalObject externalObject)
    {
        _externalObject = externalObject;
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
        if (toActive) Loaded();
        else Unloaded();
    }

    [RelayCommand]
    private void IncrementLibCount()
    {
        _externalObject.IncrementCounter();
    }

    [RelayCommand]
    private void IncrementVmCount()
    {
        WeakReferenceMessenger.Default.Send<IncrementVmCounterRequestMessage>();
    }

    sealed class IncrementVmCounterRequestMessage : RequestMessage<nint> { }

    protected override void OnActivated()
    {
        Messenger.Register<ChildViewModel, IncrementVmCounterRequestMessage>(this,
            static (r, _) => r.VmCounter++);
    }
}
