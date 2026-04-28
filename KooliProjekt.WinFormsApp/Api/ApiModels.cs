namespace KooliProjekt.WinFormsApp.Api;

public enum MediaType { Book = 1, Movie = 2, Game = 3, Music = 4, Other = 99 }

public class MediaItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? AuthorOrCreator { get; set; }
    public MediaType MediaType { get; set; } = MediaType.Book;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int CategoryId { get; set; } = 1;
}
