using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangMan.DependencyInterface
{
    public interface IInternalStorage
    {
        string GetString(string key);
        void PutString(string key, string value);
    }
}
