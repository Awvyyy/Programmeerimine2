using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers;

[Authorize]
public class PeopleController : Controller
{
    private readonly IPersonService _service;
    public PeopleController(IPersonService service) => _service = service;

    public async Task<IActionResult> Index(PersonSearch search)
    {
        return View(new PeopleIndexModel { Search = search, Data = await _service.List(search) });
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _service.Get(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    public IActionResult Create() => View(new Person());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Person item)
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
    public async Task<IActionResult> Edit(int id, Person item)
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
