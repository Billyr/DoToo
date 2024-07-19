using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using DoToo.Models;

namespace DoToo.ViewModels
{
    public partial class TodoItemViewModel : ViewModel
    {
        [ObservableProperty] TodoItem item;
        public TodoItemViewModel(TodoItem item) => Item = item;
        
        public string StatusText => Item.Completed ? "Reactivated" : "Completed";

        public event EventHandler ItemStatusChanged;

    }
}
