using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.BlazorApp.Api;

public enum MediaType { Book = 1, Movie = 2, Game = 3, Music = 4, Other = 99 }

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class MediaItem
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(150)]
    public string? AuthorOrCreator { get; set; }

    public MediaType MediaType { get; set; } = MediaType.Book;

    [Range(0, 999999)]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;

    [Range(1, int.MaxValue, ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}
