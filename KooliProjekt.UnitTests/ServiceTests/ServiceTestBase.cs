using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.UnitTests.ServiceTests;

public abstract class ServiceTestBase
{
    protected ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }
}
