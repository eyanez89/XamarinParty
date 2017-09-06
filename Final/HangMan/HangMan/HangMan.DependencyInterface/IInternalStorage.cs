namespace HangMan.DependencyInterface
{
    public interface IInternalStorage
    {
        string GetString(string key);
        void PutString(string key, string value);
    }
}
