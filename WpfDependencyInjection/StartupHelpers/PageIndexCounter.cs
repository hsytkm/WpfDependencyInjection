namespace WpfDependencyInjection.StartupHelpers;

public interface IPageIndexCounter
{
    PageIndex Index { get; }
}

/// <summary>
/// ParentPage の 子View に共通の Index を設定するためのクラスです
/// </summary>
public sealed class PageIndexCounter : IPageIndexCounter
{
    // ParentPage の作成前にインクリメントする設計にしています。
    //   Increment() -> 1
    //   Child -> 1, Parent -> 1
    //   Child -> 2, Parent -> 2

    public PageIndex Index { get; private set; } = new(0);
    public PageIndex Increment() => ++Index;
}
