using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models;

[ExcludeFromCodeCoverage]
public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

[ExcludeFromCodeCoverage] public class CategoriesIndexModel { public CategorySearch Search { get; set; } = new(); public PagedResult<Category> Data { get; set; } = new(); }
[ExcludeFromCodeCoverage] public class PeopleIndexModel { public PersonSearch Search { get; set; } = new(); public PagedResult<Person> Data { get; set; } = new(); }
[ExcludeFromCodeCoverage] public class MediaItemsIndexModel { public MediaItemSearch Search { get; set; } = new(); public PagedResult<MediaItem> Data { get; set; } = new(); }
[ExcludeFromCodeCoverage] public class LoansIndexModel { public LoanSearch Search { get; set; } = new(); public PagedResult<Loan> Data { get; set; } = new(); }
[ExcludeFromCodeCoverage] public class ReviewsIndexModel { public ReviewSearch Search { get; set; } = new(); public PagedResult<Review> Data { get; set; } = new(); }
