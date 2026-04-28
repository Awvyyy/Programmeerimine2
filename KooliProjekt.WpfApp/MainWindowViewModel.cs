using System.Collections.ObjectModel;
using System.Windows.Input;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp;

public class MainWindowViewModel : NotifyPropertyChangedBase
{
    private readonly IApiClient _apiClient;
    private MediaItem? _selectedItem;

    public ObservableCollection<MediaItem> Items { get; } = new();
    public Action<string>? OnError { get; set; }
    public Func<bool> ConfirmDelete { get; set; } = () => true;

    public MediaItem? SelectedItem
    {
        get => _selectedItem;
        set { _selectedItem = value; OnPropertyChanged(); }
    }

    public ICommand LoadCommand { get; }
    public ICommand NewCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }

    public MainWindowViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        LoadCommand = new RelayCommand(async () => await Load());
        NewCommand = new RelayCommand(New);
        SaveCommand = new RelayCommand(async () => await Save(), () => SelectedItem != null);
        DeleteCommand = new RelayCommand(async () => await Delete(), () => SelectedItem != null);
    }

    public async Task Load()
    {
        var result = await _apiClient.List();
        if (!result.IsSuccess)
        {
            OnError?.Invoke(result.Error ?? "Load failed");
            return;
        }

        Items.Clear();
        foreach (var item in result.Value ?? new List<MediaItem>())
        {
            Items.Add(item);
        }
    }

    private void New()
    {
        SelectedItem = new MediaItem { Title = "New item", CategoryId = 1 };
        Items.Add(SelectedItem);
    }

    private async Task Save()
    {
        if (SelectedItem == null) return;

        var result = await _apiClient.Save(SelectedItem);
        if (!result.IsSuccess) OnError?.Invoke(result.Error ?? "Save failed");

        await Load();
    }

    private async Task Delete()
    {
        if (SelectedItem == null) return;
        if (!ConfirmDelete()) return;

        var result = await _apiClient.Delete(SelectedItem.Id);
        if (!result.IsSuccess) OnError?.Invoke(result.Error ?? "Delete failed");

        await Load();
    }
}
