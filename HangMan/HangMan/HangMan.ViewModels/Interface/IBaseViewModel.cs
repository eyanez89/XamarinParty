using System.ComponentModel;

namespace HangMan.ViewModels
{
    interface IBaseViewModel : INotifyPropertyChanged
    {
        bool IsBusy { get; set; }
    }
}
