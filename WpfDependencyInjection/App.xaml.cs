using System.Windows;
using Reactive.Bindings;
using Reactive.Bindings.Schedulers;

namespace WpfDependencyInjection;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        ReactivePropertyScheduler.SetDefault(new ReactivePropertyWpfScheduler(Dispatcher));
        base.OnStartup(e);
    }

}
