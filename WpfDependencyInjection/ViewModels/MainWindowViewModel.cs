using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfDependencyInjection.ViewModels;

public sealed class MainWindowViewModel : ObservableObject
{
    public string Title { get; } = "WpfDependencyInjection";

    public MainWindowViewModel()
    {
        
    }
}
