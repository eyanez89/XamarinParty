using Android.App;
using Android.Content;
using HangMan.DependencyInterface;
using HangMan.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(InternalStorage))]
namespace HangMan.Droid
{
    public class InternalStorage : IInternalStorage
    {
        ISharedPreferences myCustomPreferences;

        public InternalStorage()
        {
            myCustomPreferences = Application.Context.GetSharedPreferences("MyCustomPreferences", FileCreationMode.Private);
        }

        public string GetString(string key)
        {
            return myCustomPreferences.GetString(key, string.Empty);
        }

        public void PutString(string key, string value)
        {
            ISharedPreferencesEditor myEditor = myCustomPreferences.Edit();
            myEditor.PutString(key, value);
            myEditor.Apply();
        }
    }
}