using System.Windows.Controls;
using WpfLibrary;

namespace WpfDependencyInjection.Views;

public partial class Child1View : UserControl
{
    public Child1View(IDataAccess dataAccess)
    {
        InitializeComponent();
        textBlock.Text = dataAccess.GetData();
    }
}
