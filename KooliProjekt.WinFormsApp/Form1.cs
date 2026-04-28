using KooliProjekt.WinFormsApp.Api;

namespace KooliProjekt.WinFormsApp;

public class Form1 : Form, IMediaItemView
{
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, AutoGenerateColumns = true };
    private readonly Button _loadButton = new() { Text = "Load" };
    private readonly Button _newButton = new() { Text = "New" };
    private readonly Button _saveButton = new() { Text = "Save" };
    private readonly Button _deleteButton = new() { Text = "Delete" };
    private readonly MediaItemPresenter _presenter;

    public Form1()
    {
        Text = "Media Library WinForms";
        Width = 900;
        Height = 500;

        var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
        panel.Controls.AddRange(new Control[] { _loadButton, _newButton, _saveButton, _deleteButton });

        Controls.Add(_grid);
        Controls.Add(panel);

        _presenter = new MediaItemPresenter(this, new ApiClient());

        _loadButton.Click += async (_, _) => await _presenter.Load();
        _newButton.Click += (_, _) => _presenter.New();
        _saveButton.Click += async (_, _) => await _presenter.Save();
        _deleteButton.Click += async (_, _) => await _presenter.Delete();
    }

    public IList<MediaItem> Items
    {
        get => _grid.DataSource as IList<MediaItem> ?? new List<MediaItem>();
        set => _grid.DataSource = value;
    }

    public MediaItem? SelectedItem
    {
        get => _grid.CurrentRow?.DataBoundItem as MediaItem;
        set
        {
            if (value == null) return;

            var list = Items.ToList();
            list.Add(value);
            Items = list;
        }
    }

    public void ShowError(string message) => MessageBox.Show(message, "Error");
    public bool ConfirmDelete() => MessageBox.Show("Delete selected item?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes;
}
