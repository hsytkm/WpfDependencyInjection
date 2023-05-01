using System.Windows;
using System.Windows.Controls;
using WpfDependencyInjection.ViewModels;

namespace WpfDependencyInjection.Views;

public partial class ChildView : UserControl, IIndexedPage
{
    private readonly ChildViewModel _viewModel;

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

    public ChildView(ChildViewModel viewModel)
    {
        DataContext = _viewModel = viewModel;

        InitializeComponent();
        IsVisibleChanged += UserControl_IsVisibleChanged;
    }

    private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        //Debug.WriteLine($"IsVisibleChanged(Index={Index.Index}) : {e.NewValue}");

        if (e.NewValue is bool toActive)
            _viewModel.ToggleActivation(toActive);
    }
}
