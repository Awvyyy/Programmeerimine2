using KooliProjekt.WinFormsApp;
using KooliProjekt.WinFormsApp.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WinFormsApp.UnitTests;

public class MediaItemPresenterTests
{
    [Fact]
    public async Task Load_should_put_items_to_view()
    {
        var view = new Mock<IMediaItemView>();
        var api = new Mock<IApiClient>();
        api.Setup(x => x.List()).ReturnsAsync(Result<IList<MediaItem>>.Ok(new List<MediaItem> { new MediaItem { Title = "A" } }));
        var presenter = new MediaItemPresenter(view.Object, api.Object);

        await presenter.Load();

        view.VerifySet(x => x.Items = It.Is<IList<MediaItem>>(items => items.Count == 1));
    }

    [Fact]
    public void New_should_set_selected_item()
    {
        var view = new Mock<IMediaItemView>();
        var api = new Mock<IApiClient>();
        var presenter = new MediaItemPresenter(view.Object, api.Object);

        presenter.New();

        view.VerifySet(x => x.SelectedItem = It.IsAny<MediaItem>());
    }

    [Fact]
    public async Task Delete_should_not_delete_when_user_cancels()
    {
        var view = new Mock<IMediaItemView>();
        var api = new Mock<IApiClient>();
        view.SetupGet(x => x.SelectedItem).Returns(new MediaItem { Id = 1 });
        view.Setup(x => x.ConfirmDelete()).Returns(false);

        var presenter = new MediaItemPresenter(view.Object, api.Object);
        await presenter.Delete();

        api.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
    }
}
