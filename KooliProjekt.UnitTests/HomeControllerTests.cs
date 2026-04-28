using KooliProjekt.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace KooliProjekt.UnitTests;

public class HomeControllerTests
{
    [Fact] public void Index_should_return_view() => Assert.IsType<ViewResult>(new HomeController().Index());
    [Fact] public void Privacy_should_return_view() => Assert.IsType<ViewResult>(new HomeController().Privacy());

    [Fact]
    public void Error_should_return_view()
    {
        var result = new HomeController().Error() as ViewResult;
        Assert.NotNull(result);
        Assert.NotNull(result.Model);
    }
}
