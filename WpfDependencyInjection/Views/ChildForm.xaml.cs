using System.Windows;
using WpfDependencyInjection.ViewModels;

namespace WpfDependencyInjection.Views;

public partial class ChildForm : Window
{
    public ChildForm(ChildFormViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
