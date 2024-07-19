using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoToo.Repositories;
using DoToo.Models;
using DoToo.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DoToo.ViewModels
{
    public partial class MainViewModel : ViewModel
    {
        private readonly ITodoItemRepository repository;
        private readonly IServiceProvider services;

        [ObservableProperty]
        ObservableCollection<TodoItemViewModel> items;

        public MainViewModel(ITodoItemRepository todoItemRepository, IServiceProvider services)
        {
            this.repository = todoItemRepository;

            repository.OnItemAdded += (sender, item) => 
                Items.Add(CreateTodoItemViewModel(item));

            repository.OnItemUpdated += (sender, item) =>
                Task.Run(async () => await LoadDataAsync());

            
            this.services = services;

            Task.Run(async () => await LoadDataAsync());
        }
        private async Task LoadDataAsync()
        {
            var items = await repository.GetItemAsync();
            var itemViewModels = items.Select(i => CreateTodoItemViewModel(i));

            Items = new ObservableCollection<TodoItemViewModel>(itemViewModels);
        }

        private TodoItemViewModel CreateTodoItemViewModel(TodoItem item)
        {
            var itemViewModel = new TodoItemViewModel(item);
            itemViewModel.ItemStatusChanged += ItemStatusChanged;
            return itemViewModel;
        }

        private void ItemStatusChanged(object? sender, EventArgs e)
        {
            
        }




        //public ICommand AddItemCommand => new Command(async () => {
        //    var itemView = services.GetRequiredService<ItemView>();
        //    await Navigation.PushAsync(itemView, true);
        //});

        [RelayCommand]
        private async Task AddItem()
        {
            await Navigation.PushAsync(services.GetRequiredService<ItemView>(), animated: true);
        }

    }
}
