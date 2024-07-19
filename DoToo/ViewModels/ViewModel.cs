using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DoToo.ViewModels
{
    [ObservableObject]
    public abstract partial class ViewModel //: INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler? PropertyChanged;

        //public void RaisePropertyChanged(params string[] propertyNames)
        //{
        //    foreach (var propertyName in propertyNames)
        //    {
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
        public INavigation Navigation { get; set; }
    }
}
