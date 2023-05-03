using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Options;

namespace WpfDependencyInjection.Views;

public partial class MainWindow : Window
{
    // ページ表示の最大数
    private readonly int _pagesCountMax;
    private readonly ILogger<MainWindow> _logger;
    private readonly IIndexedFactory<ParentPage> _parentFactory;
    
    private readonly List<Control> _pages = new();
    private int _displayedPagesCount = 0;

    public MainWindow(IOptions<AppSettings> appSettings, ILogger<MainWindow> logger, IIndexedFactory<ParentPage> parentFactory)
    {
        // DataContext は ViewModelLocator で設定しています（使用例です。直で設定してもよいです。）
        _pagesCountMax = appSettings.Value.PagesCountMax;
        _logger = logger;
        _parentFactory = parentFactory;
        InitializeComponent();

        AddNewPage();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e) => AddNewPage();

    // 非表示のPageが存在しなければインスタンスを追加します
    private void AddNewPage()
    {
        if (_pagesCountMax <= _displayedPagesCount)
            return;

        if (_pages.Count <= _displayedPagesCount)
        {
            _logger.LogInformation("Start-Create page instance");
            var page = _parentFactory.Create();
            _logger.LogInformation("End-Create page instance");
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

        _logger.LogInformation("Add view page. (Buffer={Buffer}, Display={DispCount})", _pages.Count, _displayedPagesCount);
    }

    // Viewのインスタンスは保持しつつ、表示だけ消します
    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
        if (_displayedPagesCount <= 0)
            return;

        static void hiddenLastPage(Grid grid)
        {
            int deleteIndex = grid.ColumnDefinitions.Count - 1;
            grid.Children.RemoveAt(deleteIndex);
            grid.ColumnDefinitions.RemoveAt(deleteIndex);
        }
        hiddenLastPage(pagesGrid);
        _displayedPagesCount--;

        _logger.LogInformation("Remove view page. (Buffer={Buffer}, Display={DispCount})", _pages.Count, _displayedPagesCount);
    }
}
