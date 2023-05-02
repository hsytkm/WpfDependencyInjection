using System.Windows;
using System.Windows.Controls;
using WpfDependencyInjection.ViewModels;

namespace WpfDependencyInjection.Views;

public partial class ChildView : UserControl, IIndexedPage
{
    private readonly ChildViewModel _viewModel;

    public PageIndex Index { get; }

    public ChildView(IPageIndexCounter counter, ChildViewModel viewModel)
    {
        Index = counter.Value;
        DataContext = _viewModel = viewModel;

        InitializeComponent();
        IsVisibleChanged += UserControl_IsVisibleChanged;
    }

    private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        //Debug.WriteLine($"IsVisibleChanged(Index={Index.Value}) : {e.NewValue}");

        if (e.NewValue is bool toActive)
            _viewModel.ToggleActivation(toActive);
    }
}
