using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data;

public static class SeedData
{
    public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        if (context.MediaItems.Any())
        {
            return;
        }

        var user = new IdentityUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true
        };

        userManager.CreateAsync(user, "Password123!").GetAwaiter().GetResult();

        var categories = new[]
        {
            new Category { Name = "Books", Description = "Books and e-books" },
            new Category { Name = "Movies", Description = "Movies and series" },
            new Category { Name = "Games", Description = "Board games and video games" },
            new Category { Name = "Music", Description = "Albums and concerts" },
            new Category { Name = "Other", Description = "Other media" }
        };
        context.Categories.AddRange(categories);

        var people = Enumerable.Range(1, 10)
            .Select(i => new Person { FullName = $"Demo Person {i}", Email = $"person{i}@example.com", PhoneNumber = $"+372 555{i:0000}" })
            .ToList();
        context.People.AddRange(people);
        context.SaveChanges();

        var titles = new[] { "Clean Code", "The Pragmatic Programmer", "Inception", "Interstellar", "Catan", "Ticket to Ride", "Kind of Blue", "Random Access Memories", "ASP.NET Notes", "Design Patterns" };
        for (var i = 0; i < titles.Length; i++)
        {
            context.MediaItems.Add(new MediaItem
            {
                Title = titles[i],
                AuthorOrCreator = i % 2 == 0 ? "Known Creator" : "School demo",
                CategoryId = categories[i % categories.Length].Id,
                MediaType = (MediaType)((i % 4) + 1),
                Price = 10 + i,
                ReleaseDate = DateTime.Today.AddYears(-i),
                IsAvailable = true
            });
        }
        context.SaveChanges();

        var media = context.MediaItems.ToList();
        for (var i = 0; i < 10; i++)
        {
            context.Loans.Add(new Loan
            {
                MediaItemId = media[i].Id,
                BorrowerId = people[i].Id,
                BorrowedAt = DateTime.Today.AddDays(-i),
                DueAt = DateTime.Today.AddDays(14 - i),
                ReturnedAt = i % 3 == 0 ? DateTime.Today : null,
                Notes = "Seed row"
            });

            context.Reviews.Add(new Review
            {
                MediaItemId = media[i].Id,
                ReviewerId = people[9 - i].Id,
                Rating = (i % 5) + 1,
                Comment = $"Demo review {i + 1}",
                CreatedAt = DateTime.Today.AddDays(-i)
            });
        }

        context.SaveChanges();
    }
}
