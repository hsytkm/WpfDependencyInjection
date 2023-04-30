using System.Windows;
using System.Windows.Controls;
using WpfDependencyInjection.ViewModels;

namespace WpfDependencyInjection.Views;

public partial class Parent1Page : UserControl, IIndexedPage
{
    private readonly Parent1PageViewModel _viewModel;

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

    public Parent1Page(Parent1PageViewModel viewModel, Child1View child1)
    {
        DataContext = _viewModel = viewModel;

        InitializeComponent();

        content.Content = child1;
        IsVisibleChanged += Parent1Page_IsVisibleChanged;
    }

    private void Parent1Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        Debug.WriteLine($"IsVisibleChanged(Index={Index.Index}) : {e.NewValue}");

        if (e.NewValue is bool toActive)
            _viewModel.ToggleActivation(toActive);
    }
}
