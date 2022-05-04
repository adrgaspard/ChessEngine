using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChessEngine.MVVM.ViewModels.Abstractions
{
    public abstract class ViewModelBase : ObservableObject, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
