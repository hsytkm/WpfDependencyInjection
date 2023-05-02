namespace WpfDependencyInjection.Common;

public readonly record struct PageIndex(int Value)
{
    //public static readonly PageIndex Zero = new(0);
    public bool IsZero() => Value == 0;

    public static PageIndex operator ++(PageIndex x) => new(x.Value + 1);
}
