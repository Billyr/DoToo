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

        [ObservableProperty]
        TodoItemViewModel selectedItem;

        [ObservableProperty]
        bool showAll;

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

            if (!ShowAll)
            {
                items = items.Where(x => x.Completed == false).ToList();
            }

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
            if (sender is TodoItemViewModel item)
            {
                if (!ShowAll && item.Item.Completed)
                    Items.Remove(item);

                Task.Run(async () => await repository.UpdateItemAsync(item.Item));

            }
        }

        [RelayCommand]
        private async Task ToggleFilterAsync()
        {
            ShowAll = !ShowAll;
            await LoadDataAsync();
        }

        partial void OnSelectedItemChanging(TodoItemViewModel value)
        {
            if (value == null)
                return;

            MainThread.BeginInvokeOnMainThread(async () => {
                await NavigateToItemAsync(value); 
            });
        }

        private async Task NavigateToItemAsync(TodoItemViewModel item)
        {
            var itemView = services.GetRequiredService<ItemView>();
            var vm = itemView.BindingContext as ItemViewModel;

            vm.Item = item.Item;
            itemView.Title = "Edit Todo Item";

            await Navigation.PushAsync(itemView);

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
