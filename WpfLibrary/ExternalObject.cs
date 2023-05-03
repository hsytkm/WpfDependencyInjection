using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfLibrary;

public class ExternalObject : IExternalObject
{
    private int _counter1;

    public string GetData()
    {
        char c = (char)('A' + _counter1++);
        return $"Group {c} from Lib";
    }

    public int Counter
    {
        get => _counter;
        private set
        {
            if (_counter != value)
                SetProperty(ref _counter, value);
        }
    }
    private int _counter;

    public void IncrementCounter() => Counter++;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }

    public void Dispose()
    {
        Debug.WriteLine("ExternalObject is disposed.");
    }
}
