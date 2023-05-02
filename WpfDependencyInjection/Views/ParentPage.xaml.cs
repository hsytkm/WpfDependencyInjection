using WpfDependencyInjection.ViewModels;

namespace WpfDependencyInjection.Views;

public partial class ParentPage : IndexedUserControl
{
    public ParentPage(IPageIndexCounter counter, ParentPageViewModel viewModel, ChildView childView)
        : base(counter, viewModel)
    {
        InitializeComponent();
        content.Content = childView;
    }
}
