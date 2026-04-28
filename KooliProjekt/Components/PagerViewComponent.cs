using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Components;

[ExcludeFromCodeCoverage]
public class PagerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(PagedResultBase result) => View(result);
}
