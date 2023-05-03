namespace WpfDependencyInjection;

/// <summary>
/// コマンドライン引数 から読み込まれる設定
/// </summary>
public sealed class CommandLineArgs
{
    // 以下のように指定します。
    //   PageCount="2"
    //   /PageCount "2"
    //   --PageCount "2"

    public int PageCount { get; set; } = 1;
}
