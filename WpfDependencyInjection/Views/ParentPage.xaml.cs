using System.Windows;
using System.Windows.Controls;
using WpfDependencyInjection.ViewModels;

namespace WpfDependencyInjection.Views;

public partial class ParentPage : UserControl, IIndexedPage
{
    private readonly ParentPageViewModel _viewModel;

    public PageIndex Index { get; }

    public ParentPage(IPageIndexCounter counter, ParentPageViewModel viewModel, ChildView childView)
    {
        Index = counter.Value;
        DataContext = _viewModel = viewModel;

        InitializeComponent();

        content.Content = childView;
        IsVisibleChanged += UserControl_IsVisibleChanged;
    }

    private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        Debug.WriteLine($"IsVisibleChanged(Index={Index.Value}) : {e.NewValue}");

        if (e.NewValue is bool toActive)
            _viewModel.ToggleActivation(toActive);
    }
}
