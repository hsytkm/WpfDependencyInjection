using System.Windows;
using System.Windows.Controls;

namespace WpfDependencyInjection.Views;

public abstract class IndexedUserControl : UserControl, IIndexedView
{
    private readonly IIndexedViewModel _viewModel;

    public PageIndex Index { get; }

    public IndexedUserControl(IPageIndexCounter counter, IIndexedViewModel viewModel)
    {
        Index = counter.Value;
        DataContext = _viewModel = viewModel;

        IsVisibleChanged += UserControl_IsVisibleChanged;
    }

    private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        //Debug.WriteLine($"IsVisibleChanged(Index={Index.Value}) : {e.NewValue}");

        if (e.NewValue is bool toActive)
            _viewModel.ToggleActivation(toActive);
    }
}
