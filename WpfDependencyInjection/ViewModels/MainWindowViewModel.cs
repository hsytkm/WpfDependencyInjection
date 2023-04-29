using System.Windows.Input;
using WpfLibrary;

namespace WpfDependencyInjection.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Message
    {
        get => _message;
        private set => SetProperty(ref _message, value);
    }
    private string _message = "";

    public ICommand GetDataCommand { get; }

    public MainWindowViewModel(IDataAccess dataAccess)
    {
        GetDataCommand = new RelayCommand(() => Message = dataAccess.GetData());
    }
}