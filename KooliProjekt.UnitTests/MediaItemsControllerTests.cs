using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests;

public class MediaItemsControllerTests
{
    private readonly Mock<IMediaItemService> _mediaService = new();
    private readonly Mock<ICategoryService> _categoryService = new();
    private readonly MediaItemsController _controller;

    public MediaItemsControllerTests()
    {
        _categoryService.Setup(x => x.All()).ReturnsAsync(new List<Category> { new Category { Id = 1, Name = "Books" } });
        _controller = new MediaItemsController(_mediaService.Object, _categoryService.Object);
    }

    [Fact]
    public async Task Index_should_return_view()
    {
        _mediaService.Setup(x => x.List(It.IsAny<MediaItemSearch>())).ReturnsAsync(new PagedResult<MediaItem>());
        var result = await _controller.Index(new MediaItemSearch());
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Details_should_return_not_found_when_item_missing()
    {
        var result = await _controller.Details(999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_should_return_view_when_item_exists()
    {
        _mediaService.Setup(x => x.Get(1)).ReturnsAsync(new MediaItem { Id = 1, Title = "Test" });
        var result = await _controller.Details(1);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_post_should_save_valid_item()
    {
        var item = new MediaItem { Title = "Test", CategoryId = 1 };
        _mediaService.Setup(x => x.Save(item)).Verifiable();

        var result = await _controller.Create(item);

        Assert.IsType<RedirectToActionResult>(result);
        _mediaService.VerifyAll();
    }

    [Fact]
    public async Task Create_post_should_return_view_when_model_invalid()
    {
        _controller.ModelState.AddModelError("Title", "Required");
        var result = await _controller.Create(new MediaItem());
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_should_call_service()
    {
        _mediaService.Setup(x => x.Delete(1)).ReturnsAsync(true).Verifiable();
        var result = await _controller.DeleteConfirmed(1);
        Assert.IsType<RedirectToActionResult>(result);
        _mediaService.VerifyAll();
    }
}
