using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaItemsApiController : ControllerBase
{
    private readonly IMediaItemService _service;
    private readonly ICategoryService _categoryService;

    public MediaItemsApiController(IMediaItemService service, ICategoryService categoryService)
    {
        _service = service;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IList<MediaItemApiModel>>> Get()
    {
        var result = await _service.List(new MediaItemSearch { Page = 1, PageSize = 1000 });
        return Ok(result.Results.Select(ToApiModel).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MediaItemApiModel>> Get(int id)
    {
        var item = await _service.Get(id);
        return item == null ? NotFound() : Ok(ToApiModel(item));
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IList<CategoryApiModel>>> Categories()
    {
        var categories = await _categoryService.All();
        return Ok(categories.Select(ToApiModel).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<MediaItemApiModel>> Post(MediaItem item)
    {
        await _service.Save(item);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, ToApiModel(item));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, MediaItem item)
    {
        if (id != item.Id) return BadRequest();

        var existing = await _service.Get(id);
        if (existing == null) return NotFound();

        await _service.Save(item);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.Delete(id);
        return deleted ? NoContent() : NotFound();
    }

    private static MediaItemApiModel ToApiModel(MediaItem item)
    {
        return new MediaItemApiModel
        {
            Id = item.Id,
            Title = item.Title,
            AuthorOrCreator = item.AuthorOrCreator,
            MediaType = item.MediaType,
            ReleaseDate = item.ReleaseDate,
            Price = item.Price,
            IsAvailable = item.IsAvailable,
            CategoryId = item.CategoryId,
            Category = item.Category == null ? null : ToApiModel(item.Category)
        };
    }

    private static CategoryApiModel ToApiModel(Category category)
    {
        return new CategoryApiModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }
}

public class MediaItemApiModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? AuthorOrCreator { get; set; }
    public MediaType MediaType { get; set; } = MediaType.Book;
    public DateTime? ReleaseDate { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int CategoryId { get; set; }
    public CategoryApiModel? Category { get; set; }
}

public class CategoryApiModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
