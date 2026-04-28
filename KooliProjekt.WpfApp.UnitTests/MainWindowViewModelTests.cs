using KooliProjekt.WpfApp;
using KooliProjekt.WpfApp.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WpfApp.UnitTests;

public class MainWindowViewModelTests
{
    [Fact]
    public async Task Load_should_fill_items()
    {
        var api = new Mock<IApiClient>();
        api.Setup(x => x.List()).ReturnsAsync(Result<IList<MediaItem>>.Ok(new List<MediaItem> { new MediaItem { Title = "A" } }));
        var vm = new MainWindowViewModel(api.Object);

        await vm.Load();

        Assert.Single(vm.Items);
    }

    [Fact]
    public void NewCommand_should_create_selected_item()
    {
        var api = new Mock<IApiClient>();
        var vm = new MainWindowViewModel(api.Object);

        vm.NewCommand.Execute(null);

        Assert.NotNull(vm.SelectedItem);
    }

    [Fact]
    public async Task DeleteCommand_should_use_confirmation()
    {
        var api = new Mock<IApiClient>();
        api.Setup(x => x.List()).ReturnsAsync(Result<IList<MediaItem>>.Ok(new List<MediaItem>()));
        var vm = new MainWindowViewModel(api.Object)
        {
            SelectedItem = new MediaItem { Id = 1 },
            ConfirmDelete = () => false
        };

        vm.DeleteCommand.Execute(null);

        await Task.Delay(50);
        api.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
    }
}
