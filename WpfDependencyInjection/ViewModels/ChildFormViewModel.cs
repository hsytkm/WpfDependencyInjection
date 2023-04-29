using WpfLibrary;

namespace WpfDependencyInjection.ViewModels;

public class ChildFormViewModel : ViewModelBase
{
    public string Message { get; }

    public ChildFormViewModel(IDataAccess dataAccess)
    {
        Message = dataAccess.GetData();
    }
}