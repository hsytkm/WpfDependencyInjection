using System.Windows;
using System.Windows.Controls;
using WpfDependencyInjection.ViewModels;

namespace WpfDependencyInjection.Views;

public partial class Child1View : UserControl
{
    private readonly Child1ViewModel _viewModel;

    public Child1View(Child1ViewModel viewModel)
    {
        DataContext = _viewModel = viewModel;

        InitializeComponent();
        IsVisibleChanged += Parent1Page_IsVisibleChanged;
    }

    private void Parent1Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        //Debug.WriteLine($"IsVisibleChanged(Index={Index.Index}) : {e.NewValue}");

        if (e.NewValue is bool toActive)
            _viewModel.ToggleActivation(toActive);
    }
}
