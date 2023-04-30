namespace WpfDependencyInjection.Common;

public readonly record struct PageIndex(int Index)
{
    //public static readonly PageIndex Zero = new(0);
    public bool IsZero() => Index == 0;
}
