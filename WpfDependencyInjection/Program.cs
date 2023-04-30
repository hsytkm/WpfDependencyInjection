using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfDependencyInjection.StartupHelpers;
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

        App app = new();
        app.InitializeComponent();
        app.MainWindow = mainWindow;
        app.Run();
    }

    internal static object GetViewModel(Type viewType) => _host!.Services.GetRequiredViewModel(viewType);

    private static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddViewAndViewModel<MainWindow, MainWindowViewModel>();

                services.AddTransientFactory<Parent1Page>();
                services.AddTransient<Parent1PageViewModel>();
                services.AddTransient<Child1View>();

                //services.AddTransient<ChildFormViewModel>();
                services.AddSingleton<IDataAccess, DataAccess>();
            });
}
