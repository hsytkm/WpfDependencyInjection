namespace WpfDependencyInjection;

/// <summary>
/// コマンドライン引数 から読み込まれる設定
/// </summary>
public sealed record CommandLineArgs
{
    // 以下のように指定します。
    //   PageCount="2"
    //   /PageCount "2"
    //   --PageCount "2"

    // 直で取得するコード例
    // var args = host.Services.GetRequiredService<IOptions<CommandLineArgs>>().Value;

    public int PageCount { get; init; } = 1;
}
