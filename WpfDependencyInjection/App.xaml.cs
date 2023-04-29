using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfDependencyInjection.StartupHelpers;
using WpfDependencyInjection.Views;

namespace WpfDependencyInjection;

// [Dependency Injection in WPF in .NET 6 Including the Factory Pattern - YouTube](https://www.youtube.com/watch?v=dLR_D2IJE1M&ab_channel=IAmTimCorey)

public partial class App : Application
{
    internal static IHost? Host { get; private set; }

    public App()
    {
        Host = AppHost.CreateHostBuilder().Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await Host!.StartAsync();

        MainWindow = Host.Services.GetRequiredService<MainWindow>();
        MainWindow.Visibility = Visibility.Visible;

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await Host!.StopAsync();
        Host!.Dispose();

        base.OnExit(e);
    }

    internal static object GetViewModel(Type viewType)
        => Host!.Services.GetViewModel(viewType);
}
