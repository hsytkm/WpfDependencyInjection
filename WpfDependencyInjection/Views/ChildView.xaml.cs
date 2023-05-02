using WpfDependencyInjection.ViewModels;

namespace WpfDependencyInjection.Views;

public partial class ChildView : IndexedUserControl
{
    public ChildView(IPageIndexCounter counter, ChildViewModel viewModel)
        : base(counter, viewModel)
    {
        InitializeComponent();
    }
}
