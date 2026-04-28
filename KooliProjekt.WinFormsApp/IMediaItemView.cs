using KooliProjekt.WinFormsApp.Api;

namespace KooliProjekt.WinFormsApp;

public interface IMediaItemView
{
    IList<MediaItem> Items { get; set; }
    MediaItem? SelectedItem { get; set; }
    void ShowError(string message);
    bool ConfirmDelete();
}
