using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Controllers;

[Authorize]
public class MediaItemsController : Controller
{
    private readonly IMediaItemService _service;
    private readonly ICategoryService _categoryService;

    public MediaItemsController(IMediaItemService service, ICategoryService categoryService)
    {
        _service = service;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(MediaItemSearch search)
    {
        ViewData["Categories"] = new SelectList(await _categoryService.All(), "Id", "Name", search.CategoryId);
        return View(new MediaItemsIndexModel { Search = search, Data = await _service.List(search) });
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _service.Get(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    public async Task<IActionResult> Create()
    {
        await FillDropDowns();
        return View(new MediaItem { ReleaseDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MediaItem item)
    {
        if (!ModelState.IsValid)
        {
            await FillDropDowns(item.CategoryId);
            return View(item);
        }

        await _service.Save(item);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _service.Get(id.Value);
        if (item == null) return NotFound();
        await FillDropDowns(item.CategoryId);
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MediaItem item)
    {
        if (id != item.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            await FillDropDowns(item.CategoryId);
            return View(item);
        }

        await _service.Save(item);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _service.Get(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _service.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task FillDropDowns(int? selectedCategoryId = null)
    {
        ViewData["CategoryId"] = new SelectList(await _categoryService.All(), "Id", "Name", selectedCategoryId);
    }
}
