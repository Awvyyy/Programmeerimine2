using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data;

public class PagedResultBase
{
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public int RowCount { get; set; }
}

public class PagedResult<T> : PagedResultBase where T : class
{
    public IList<T> Results { get; set; } = new List<T>();
}

public static class PagingExtensions
{
    public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, int page, int pageSize) where T : class
    {
        page = page < 1 ? 1 : page;

        var result = new PagedResult<T>
        {
            CurrentPage = page,
            PageSize = pageSize,
            RowCount = await query.CountAsync()
        };

        result.PageCount = (int)Math.Ceiling((double)result.RowCount / pageSize);
        result.Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return result;
    }
}
