using System.Windows;
using WpfDependencyInjection.StartupHelpers;

namespace WpfDependencyInjection.Views;

public partial class MainWindow : Window
{
    private readonly IAbstractFactory<ChildForm> _childFormFactory;

    public MainWindow(IAbstractFactory<ChildForm> childFormFactory)
    {
        InitializeComponent();
        _childFormFactory = childFormFactory;
    }

    private void OpenChildForm_Click(object sender, RoutedEventArgs e)
    {
        // Create の度に新しいインスタンスが生成されます
        // (DI でインスタンス自体を差し込む実装では、このようなことが実現できません)
        _childFormFactory.Create().Show();
    }
}