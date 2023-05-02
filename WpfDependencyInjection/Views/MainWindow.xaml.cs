using System.Windows;
using System.Windows.Controls;

namespace WpfDependencyInjection.Views;

public partial class MainWindow : Window
{
    private readonly IIndexedFactory<ParentPage> _parentFactory;
    private readonly List<Control> _pages = new();
    private int _displayedPagesCount = 0;

    public MainWindow(IIndexedFactory<ParentPage> parentFactory)
    {
        // DataContext は ViewModelLocator で設定しています（使用例です。直で設定してもよいです。）
        _parentFactory = parentFactory;
        InitializeComponent();

        AddNewPage();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e) => AddNewPage();

    private void AddNewPage()
    {
        // 非表示のPageが存在しなければインスタンスを追加します
        if (_pages.Count <= _displayedPagesCount)
        {
            var page = _parentFactory.Create();
            _pages.Add(page);
        }

        static void addLastPage(Grid grid, Control panel)
        {
            Grid.SetColumn(panel, grid.ColumnDefinitions.Count);

            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(1d, GridUnitType.Star)
            });
            grid.Children.Add(panel);
        }
        addLastPage(pagesGrid, _pages[_displayedPagesCount]);
        _displayedPagesCount++;

        Debug.WriteLine($"Add: PagesCount={_pages.Count}, DispCount={_displayedPagesCount}");
    }

    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
        // Viewのインスタンスは保持しつつ、表示だけ消します
        if (_displayedPagesCount > 0)
        {
            static void hiddenLastPage(Grid grid)
            {
                int deleteIndex = grid.ColumnDefinitions.Count - 1;
                grid.Children.RemoveAt(deleteIndex);
                grid.ColumnDefinitions.RemoveAt(deleteIndex);
            }
            hiddenLastPage(pagesGrid);
            _displayedPagesCount--;
        }

        Debug.WriteLine($"Remove: PagesCount={_pages.Count}, DispCount={_displayedPagesCount}");
    }
}
