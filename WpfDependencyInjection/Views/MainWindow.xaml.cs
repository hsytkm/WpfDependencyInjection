using System.ComponentModel;
using System.Windows;
using WpfDependencyInjection.StartupHelpers;

namespace WpfDependencyInjection.Views;

public partial class MainWindow : Window
{
    private readonly IAbstractFactory<ChildForm> _childFormFactory;

    public MainWindow(IAbstractFactory<ChildForm> childFormFactory)
    {
        _childFormFactory = childFormFactory;

        InitializeComponent();
        Closing += MainWindow_Closing;
    }

    private void OpenChildForm_Click(object sender, RoutedEventArgs e)
    {
        // Create の度に新しいインスタンスが生成されます
        // (DI でインスタンス自体を差し込む実装では、このような動作を実現できません)
        _childFormFactory.Create().Show();
    }

    private void MainWindow_Closing(object? sender, CancelEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Confirm Shutdown?", "MessageBox.Show", MessageBoxButton.OKCancel);
        if (result is MessageBoxResult.Cancel)
            e.Cancel = true;
    }
}
