using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfDependencyInjection.ViewModels;
using WpfDependencyInjection.Views;
using WpfLibrary;

namespace WpfDependencyInjection;

public static class Program
{
    private static IHost? _host;

    [STAThread]
    private static void Main(string[] args)
    {
        using var host = _host = CreateHostBuilder(args).Build();
        host.Start();

        var mainWindow = host.Services.GetRequiredService<MainWindow>();
        mainWindow.Visibility = Visibility.Visible;
        mainWindow.Closing += (_, e) =>
        {
            if (e.Cancel) return;
            Debug.WriteLine("MainWindowClosing()");
        };

        var app = host.Services.GetRequiredService<App>();
        app.MainWindow = mainWindow;
        app.InitializeComponent();
        app.Run();
    }

    /// <summary>
    /// ViewModelLocator から使用されます。(直で DataContext を設定する場合は必要ありません)
    /// </summary>
    internal static object GetViewModel(Type viewType) => _host!.Services.GetRequiredViewModel(viewType);

    private static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));

                services.AddSingleton<App>();
                services.AddViewAndViewModel<MainWindow, MainWindowViewModel>();

                services.AddSingleton<IExternalObject, ExternalObject>();

                services.AddSingleton<IPageIndexCounter, PageIndexCounter>();
                services.AddIndexedFactory<ParentPage>();
                services.AddTransient<ParentPageViewModel>();
                services.AddViewAndViewModel<ChildView, ChildViewModel>();

            });
}
