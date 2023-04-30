using System.Diagnostics;
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
            Debug.WriteLine("MainWindowClosing()");     //-3
        };

        App app = new();
        app.InitializeComponent();
        app.MainWindow = mainWindow;
        app.Exit += (_, _) => Debug.WriteLine("AppExit() *cannot be canceled.");    //-2
        app.Run();

        Debug.WriteLine("End Main()");  //-1
    }

    internal static object GetViewModel(Type viewType) => _host!.Services.GetRequiredViewModel(viewType);

    private static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddViewAndViewModel<MainWindow, MainWindowViewModel>();
                services.AddTransientFactory<ChildForm>();
                services.AddTransient<ChildFormViewModel>();
                services.AddSingleton<IDataAccess, DataAccess>();

            });
}
