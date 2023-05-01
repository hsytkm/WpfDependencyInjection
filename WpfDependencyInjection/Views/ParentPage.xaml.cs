using System.Windows;
using System.Windows.Controls;
using WpfDependencyInjection.ViewModels;

namespace WpfDependencyInjection.Views;

public partial class ParentPage : UserControl, IIndexedPage
{
    private readonly ParentPageViewModel _viewModel;

    public PageIndex Index
    {
        get => _index;
        internal set
        {
            if (value.IsZero())
                throw new ArgumentException("Index zero isn't allowed.", nameof(value));

            if (!_index.IsZero())
                throw new InvalidOperationException("Index is already set.");

            _viewModel.Index = _index = value;
        }
    }
    private PageIndex _index;

    public ParentPage(ParentPageViewModel viewModel, ChildView childView)
    {
        DataContext = _viewModel = viewModel;

        InitializeComponent();

        content.Content = childView;
        IsVisibleChanged += UserControl_IsVisibleChanged;
    }

    private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        Debug.WriteLine($"IsVisibleChanged(Index={Index.Index}) : {e.NewValue}");

        if (e.NewValue is bool toActive)
            _viewModel.ToggleActivation(toActive);
    }
}
