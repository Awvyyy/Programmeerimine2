using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Data;

public enum MediaType
{
    Book = 1,
    Movie = 2,
    Game = 3,
    Music = 4,
    Other = 99
}

[ExcludeFromCodeCoverage]
public class Category
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    // One category can contain many media items.
    public IList<MediaItem> MediaItems { get; set; } = new List<MediaItem>();
}

[ExcludeFromCodeCoverage]
public class Person
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress, StringLength(200)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? PhoneNumber { get; set; }

    public IList<Loan> Loans { get; set; } = new List<Loan>();
    public IList<Review> Reviews { get; set; } = new List<Review>();
}

[ExcludeFromCodeCoverage]
public class MediaItem
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(150)]
    public string? AuthorOrCreator { get; set; }

    public MediaType MediaType { get; set; } = MediaType.Book;

    [DataType(DataType.Date)]
    public DateTime? ReleaseDate { get; set; }

    [Range(0, 999999)]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public IList<Loan> Loans { get; set; } = new List<Loan>();
    public IList<Review> Reviews { get; set; } = new List<Review>();
}

[ExcludeFromCodeCoverage]
public class Loan
{
    public int Id { get; set; }

    public int MediaItemId { get; set; }
    public MediaItem? MediaItem { get; set; }

    public int BorrowerId { get; set; }
    public Person? Borrower { get; set; }

    public DateTime BorrowedAt { get; set; } = DateTime.Now;
    public DateTime DueAt { get; set; } = DateTime.Now.AddDays(14);
    public DateTime? ReturnedAt { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}

[ExcludeFromCodeCoverage]
public class Review
{
    public int Id { get; set; }

    public int MediaItemId { get; set; }
    public MediaItem? MediaItem { get; set; }

    public int ReviewerId { get; set; }
    public Person? Reviewer { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; } = 5;

    [StringLength(1000)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
