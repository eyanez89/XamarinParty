using Foundation;
using HangMan.DependencyInterface;
using HangMan.iOS.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(InternalStorage))]
namespace HangMan.iOS.Implementations
{
    public class InternalStorage : IInternalStorage
    {
        public string GetString(string key)
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey(key);
        }

        public void PutString(string key, string value)
        {
            NSUserDefaults.StandardUserDefaults.SetString(value, key);
        }
    }
}
