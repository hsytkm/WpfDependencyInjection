using System.ComponentModel;
using System.Windows;

namespace WpfDependencyInjection.Views;

/// <summary>
/// 添付プロパティで DataContext を初期化します。
/// これを使用すると楽ですが、ctorで直接DIを経由して DataContext を設定した方が高速だと思います。
///   local:ViewModelLocator.AutoWireViewModel="True"
/// </summary>
public static class ViewModelLocator
{
    public static bool GetAutoWireViewModel(DependencyObject obj) =>
        (bool)obj.GetValue(AutoWireViewModelProperty);

    public static void SetAutoWireViewModel(DependencyObject obj, bool value) =>
        obj.SetValue(AutoWireViewModelProperty, value);

    public static readonly DependencyProperty AutoWireViewModelProperty =
        DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.NotDataBindable, OnAutoWireViewModelChanged));

    private static void OnAutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(d))
            return;

        if (d is not FrameworkElement element)
            return;

        if (e.NewValue is bool and true)
        {
            var viewType = element.GetType();
            element.DataContext = Program.GetViewModel(viewType);
        }
    }
}
