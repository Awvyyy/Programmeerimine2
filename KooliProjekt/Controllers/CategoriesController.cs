using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly ICategoryService _service;
    public CategoriesController(ICategoryService service) => _service = service;

    public async Task<IActionResult> Index(CategorySearch search)
    {
        return View(new CategoriesIndexModel { Search = search, Data = await _service.List(search) });
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _service.Get(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    public IActionResult Create() => View(new Category());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category item)
    {
        if (!ModelState.IsValid) return View(item);
        await _service.Save(item);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _service.Get(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category item)
    {
        if (id != item.Id) return NotFound();
        if (!ModelState.IsValid) return View(item);
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
}
