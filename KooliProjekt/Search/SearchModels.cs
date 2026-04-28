using KooliProjekt.Data;

namespace KooliProjekt.Search;

public class CategorySearch { public string? Keyword { get; set; } public int Page { get; set; } = 1; public int PageSize { get; set; } = 5; }
public class PersonSearch { public string? Keyword { get; set; } public int Page { get; set; } = 1; public int PageSize { get; set; } = 5; }

public class MediaItemSearch
{
    public string? Keyword { get; set; }
    public int? CategoryId { get; set; }
    public MediaType? MediaType { get; set; }
    public bool? OnlyAvailable { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}

public class LoanSearch
{
    public string? Keyword { get; set; }
    public bool? OnlyOpen { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}

public class ReviewSearch
{
    public string? Keyword { get; set; }
    public int? Rating { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}
