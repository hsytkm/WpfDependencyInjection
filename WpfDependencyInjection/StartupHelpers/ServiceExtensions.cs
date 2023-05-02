using System.ComponentModel;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace WpfDependencyInjection.StartupHelpers;

internal static class ServiceExtensions
{
    private static readonly Dictionary<Type, Type> _bindingViewAndViewModelDictionary = new();

    /// <summary>
    /// View と ViewModel を関連付けて登録します
    /// </summary>
    internal static void AddViewAndViewModel<TView, TViewModel>(this IServiceCollection services)
        where TView : FrameworkElement
        where TViewModel : class, INotifyPropertyChanged
    {
        services.AddTransient<TView>();
        services.AddTransient<TViewModel>();
        _bindingViewAndViewModelDictionary.Add(typeof(TView), typeof(TViewModel));
    }

#if false   // 動作確認しましたが使用していません(サンプルとしてややこしいので)
    /// <summary>
    /// T を生成する Factory を登録します
    /// </summary>
    internal static void AddViewFactoryWithViewModel<TView, TViewModel>(this IServiceCollection services)
        where TView : FrameworkElement
        where TViewModel : class, INotifyPropertyChanged
    {
        AddViewAndViewModel<TView, TViewModel>(services);
        services.AddSingleton<Func<TView>>(static x => () => x.GetService<TView>()!);
        services.AddSingleton<IAbstractFactory<TView>, AbstractFactory<TView>>();
    }
#endif

    /// <summary>
    /// 登録済みの View から ViewModel のインスタンスを取得します。 ViewModelLocator から利用されます。
    /// </summary>
    internal static object GetRequiredViewModel(this IServiceProvider provider, Type viewType)
        => _bindingViewAndViewModelDictionary.TryGetValue(viewType, out Type? viewModelType)
            ? provider.GetRequiredService(viewModelType)!
            : throw new KeyNotFoundException(viewType.ToString());

}
