using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests;

public class MediaItemServiceTests : ServiceTestBase
{
    [Fact]
    public async Task Save_should_add_new_object()
    {
        using var context = CreateContext();
        context.Categories.Add(new Category { Id = 1, Name = "Books" });
        await context.SaveChangesAsync();
        var service = new MediaItemService(context);

        await service.Save(new MediaItem { Title = "New item", CategoryId = 1 });

        Assert.Single(context.MediaItems);
    }

    [Fact]
    public async Task Save_should_update_existing_object()
    {
        using var context = CreateContext();
        context.Categories.Add(new Category { Id = 1, Name = "Books" });
        context.MediaItems.Add(new MediaItem { Id = 1, Title = "Old", CategoryId = 1 });
        await context.SaveChangesAsync();
        var service = new MediaItemService(context);

        await service.Save(new MediaItem { Id = 1, Title = "New", CategoryId = 1 });

        Assert.Equal("New", context.MediaItems.Single().Title);
    }

    [Fact]
    public async Task List_should_filter_by_keyword()
    {
        using var context = CreateContext();
        context.Categories.Add(new Category { Id = 1, Name = "Books" });
        context.MediaItems.Add(new MediaItem { Title = "Clean Code", CategoryId = 1 });
        context.MediaItems.Add(new MediaItem { Title = "Other", CategoryId = 1 });
        await context.SaveChangesAsync();
        var service = new MediaItemService(context);

        var result = await service.List(new MediaItemSearch { Keyword = "Clean" });

        Assert.Single(result.Results);
    }

    [Fact]
    public async Task Get_should_return_existing_object()
    {
        using var context = CreateContext();
        context.Categories.Add(new Category { Id = 1, Name = "Books" });
        context.MediaItems.Add(new MediaItem { Id = 1, Title = "Clean Code", CategoryId = 1 });
        await context.SaveChangesAsync();
        var service = new MediaItemService(context);

        var result = await service.Get(1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task Delete_should_return_false_when_object_missing()
    {
        using var context = CreateContext();
        var service = new MediaItemService(context);

        var result = await service.Delete(999);

        Assert.False(result);
    }

    [Fact]
    public async Task Delete_should_remove_existing_object()
    {
        using var context = CreateContext();
        context.Categories.Add(new Category { Id = 1, Name = "Books" });
        context.MediaItems.Add(new MediaItem { Id = 1, Title = "Clean Code", CategoryId = 1 });
        await context.SaveChangesAsync();
        var service = new MediaItemService(context);

        var result = await service.Delete(1);

        Assert.True(result);
        Assert.Empty(context.MediaItems);
    }
}
