using System.ComponentModel;

namespace WpfLibrary;

public interface IExternalObject : INotifyPropertyChanged
{
    int Counter { get; }
    void IncrementCounter();
    string GetData();
}
