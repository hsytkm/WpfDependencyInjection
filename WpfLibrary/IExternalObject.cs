using System.ComponentModel;

namespace WpfLibrary;

public interface IExternalObject : INotifyPropertyChanged, IDisposable
{
    int Counter { get; }
    void IncrementCounter();
    string GetData();
}
