using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfDependencyInjection.ViewModels;
using WpfDependencyInjection.Views;
using WpfLibrary;

namespace WpfDependencyInjection.StartupHelpers;

internal static class AppHost
{
    internal static IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
        .ConfigureServices((hostContext, services) =>
        {
            services.AddViewAndViewModel<MainWindow, MainWindowViewModel>();
            services.AddTransientFactory<ChildForm>();
            services.AddTransient<ChildFormViewModel>();
            services.AddSingleton<IDataAccess, DataAccess>();

        });
}
