namespace WpfDependencyInjection;

/// <summary>
/// appsettings.json から読み込まれる設定
/// </summary>
public class AppSettings
{
    // 直で取得するコード例
    // var appSettings = host.Services.GetRequiredService<IOptions<AppSettings>>().Value;

    public int PagesCountMax { get; set; } = 2;
}
