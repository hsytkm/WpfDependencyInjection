namespace WpfLibrary;

public class DataAccess : IDataAccess
{
    private int _counter;

    public string GetData()
    {
        return $"Data {++_counter} from Library";
    }
}