using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfDependencyInjection.StartupHelpers;
using WpfDependencyInjection.Views;

namespace WpfDependencyInjection;

// [Dependency Injection in WPF in .NET 6 Including the Factory Pattern - YouTube](https://www.youtube.com/watch?v=dLR_D2IJE1M&ab_channel=IAmTimCorey)

public partial class App : Application
{
    internal static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = AppHostBuilder.Create();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        AppHost!.Dispose();

        base.OnExit(e);
    }

    internal static object GetViewModel(Type viewType)
        => AppHost!.Services.GetViewModel(viewType);
}
