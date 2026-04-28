using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly IReviewService _service;
    private readonly IMediaItemService _mediaService;
    private readonly IPersonService _personService;

    public ReviewsController(IReviewService service, IMediaItemService mediaService, IPersonService personService)
    {
        _service = service;
        _mediaService = mediaService;
        _personService = personService;
    }

    public async Task<IActionResult> Index(ReviewSearch search) => View(new ReviewsIndexModel { Search = search, Data = await _service.List(search) });

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _service.Get(id.Value);
        return item == null ? NotFound() : View(item);
    }

    public async Task<IActionResult> Create()
    {
        await FillDropDowns();
        return View(new Review());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Review item)
    {
        if (!ModelState.IsValid) { await FillDropDowns(item.MediaItemId, item.ReviewerId); return View(item); }
        await _service.Save(item);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _service.Get(id.Value);
        if (item == null) return NotFound();
        await FillDropDowns(item.MediaItemId, item.ReviewerId);
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Review item)
    {
        if (id != item.Id) return NotFound();
        if (!ModelState.IsValid) { await FillDropDowns(item.MediaItemId, item.ReviewerId); return View(item); }
        await _service.Save(item);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _service.Get(id.Value);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _service.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task FillDropDowns(int? mediaId = null, int? personId = null)
    {
        ViewData["MediaItemId"] = new SelectList(await _mediaService.All(), "Id", "Title", mediaId);
        ViewData["ReviewerId"] = new SelectList(await _personService.All(), "Id", "FullName", personId);
    }
}
