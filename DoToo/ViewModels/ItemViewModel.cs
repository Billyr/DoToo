using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoToo.Models;
using DoToo.Repositories;

namespace DoToo.ViewModels
{
    public partial class ItemViewModel : ViewModel
    {
        private readonly ITodoItemRepository repository;

        [ObservableProperty] TodoItem item;

        public ItemViewModel(ITodoItemRepository _repository)
        {
            repository = _repository;
            Item = new TodoItem { Due = DateTime.Now.AddDays(1) };
        }

        [RelayCommand]
        public async Task Save()
        {
            await repository.AddOrUpdateAsync(Item);
            await Navigation.PopAsync();
        }

    }
}
