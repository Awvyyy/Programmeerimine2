using KooliProjekt.WinFormsApp.Api;

namespace KooliProjekt.WinFormsApp;

public class MediaItemPresenter
{
    private readonly IMediaItemView _view;
    private readonly IApiClient _apiClient;

    public MediaItemPresenter(IMediaItemView view, IApiClient apiClient)
    {
        _view = view;
        _apiClient = apiClient;
    }

    public async Task Load()
    {
        var result = await _apiClient.List();
        if (!result.IsSuccess)
        {
            _view.ShowError(result.Error ?? "Load failed");
            return;
        }

        _view.Items = result.Value ?? new List<MediaItem>();
    }

    public void New()
    {
        _view.SelectedItem = new MediaItem { Title = "New item", CategoryId = 1 };
    }

    public async Task Save()
    {
        if (_view.SelectedItem == null) return;

        var result = await _apiClient.Save(_view.SelectedItem);
        if (!result.IsSuccess) _view.ShowError(result.Error ?? "Save failed");

        await Load();
    }

    public async Task Delete()
    {
        if (_view.SelectedItem == null) return;
        if (!_view.ConfirmDelete()) return;

        var result = await _apiClient.Delete(_view.SelectedItem.Id);
        if (!result.IsSuccess) _view.ShowError(result.Error ?? "Delete failed");

        await Load();
    }
}
