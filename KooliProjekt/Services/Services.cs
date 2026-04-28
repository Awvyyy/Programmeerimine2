using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    public CategoryService(ApplicationDbContext context) => _context = context;

    public async Task<PagedResult<Category>> List(CategorySearch search)
    {
        var query = _context.Categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.Keyword))
            query = query.Where(x => x.Name.Contains(search.Keyword) || (x.Description != null && x.Description.Contains(search.Keyword)));

        return await query.OrderBy(x => x.Name).GetPagedAsync(search.Page, search.PageSize);
    }

    public async Task<IList<Category>> All() => await _context.Categories.OrderBy(x => x.Name).ToListAsync();
    public async Task<Category?> Get(int id) => await _context.Categories.Include(x => x.MediaItems).FirstOrDefaultAsync(x => x.Id == id);

    public async Task Save(Category category)
    {
        if (category.Id == 0)
        {
            _context.Categories.Add(category);
        }
        else
        {
            var existing = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            if (existing == null)
                _context.Categories.Update(category);
            else
                _context.Entry(existing).CurrentValues.SetValues(category);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var item = await _context.Categories.FindAsync(id);
        if (item == null) return false;
        _context.Categories.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class PersonService : IPersonService
{
    private readonly ApplicationDbContext _context;
    public PersonService(ApplicationDbContext context) => _context = context;

    public async Task<PagedResult<Person>> List(PersonSearch search)
    {
        var query = _context.People.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search.Keyword))
            query = query.Where(x => x.FullName.Contains(search.Keyword) || (x.Email != null && x.Email.Contains(search.Keyword)));
        return await query.OrderBy(x => x.FullName).GetPagedAsync(search.Page, search.PageSize);
    }

    public async Task<IList<Person>> All() => await _context.People.OrderBy(x => x.FullName).ToListAsync();
    public async Task<Person?> Get(int id) => await _context.People.Include(x => x.Loans).Include(x => x.Reviews).FirstOrDefaultAsync(x => x.Id == id);

    public async Task Save(Person person)
    {
        if (person.Id == 0)
        {
            _context.People.Add(person);
        }
        else
        {
            var existing = await _context.People.FirstOrDefaultAsync(x => x.Id == person.Id);

            if (existing == null)
                _context.People.Update(person);
            else
                _context.Entry(existing).CurrentValues.SetValues(person);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var item = await _context.People.FindAsync(id);
        if (item == null) return false;
        _context.People.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class MediaItemService : IMediaItemService
{
    private readonly ApplicationDbContext _context;
    public MediaItemService(ApplicationDbContext context) => _context = context;

    public async Task<PagedResult<MediaItem>> List(MediaItemSearch search)
    {
        var query = _context.MediaItems.Include(x => x.Category).AsQueryable();

        // Step by step is easier for me to read while learning LINQ.
        if (!string.IsNullOrWhiteSpace(search.Keyword))
            query = query.Where(x => x.Title.Contains(search.Keyword) || (x.AuthorOrCreator != null && x.AuthorOrCreator.Contains(search.Keyword)));

        if (search.CategoryId.HasValue) query = query.Where(x => x.CategoryId == search.CategoryId.Value);
        if (search.MediaType.HasValue) query = query.Where(x => x.MediaType == search.MediaType.Value);
        if (search.OnlyAvailable == true) query = query.Where(x => x.IsAvailable);

        return await query.OrderBy(x => x.Title).GetPagedAsync(search.Page, search.PageSize);
    }

    public async Task<IList<MediaItem>> All() => await _context.MediaItems.OrderBy(x => x.Title).ToListAsync();

    public async Task<MediaItem?> Get(int id)
    {
        return await _context.MediaItems
            .Include(x => x.Category)
            .Include(x => x.Loans).ThenInclude(x => x.Borrower)
            .Include(x => x.Reviews).ThenInclude(x => x.Reviewer)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task Save(MediaItem item)
    {
        if (item.Id == 0)
        {
            _context.MediaItems.Add(item);
        }
        else
        {
            // In tests and in PUT actions the same row can already be tracked.
            // Updating the tracked entity avoids the classic EF Core duplicate tracking error.
            var existing = await _context.MediaItems.FirstOrDefaultAsync(x => x.Id == item.Id);

            if (existing == null)
                _context.MediaItems.Update(item);
            else
                _context.Entry(existing).CurrentValues.SetValues(item);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var item = await _context.MediaItems.FindAsync(id);
        if (item == null) return false;
        _context.MediaItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class LoanService : ILoanService
{
    private readonly ApplicationDbContext _context;
    public LoanService(ApplicationDbContext context) => _context = context;

    public async Task<PagedResult<Loan>> List(LoanSearch search)
    {
        var query = _context.Loans.Include(x => x.MediaItem).Include(x => x.Borrower).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search.Keyword))
            query = query.Where(x => (x.MediaItem != null && x.MediaItem.Title.Contains(search.Keyword)) || (x.Borrower != null && x.Borrower.FullName.Contains(search.Keyword)) || (x.Notes != null && x.Notes.Contains(search.Keyword)));
        if (search.OnlyOpen == true) query = query.Where(x => x.ReturnedAt == null);
        return await query.OrderByDescending(x => x.BorrowedAt).GetPagedAsync(search.Page, search.PageSize);
    }

    public async Task<Loan?> Get(int id) => await _context.Loans.Include(x => x.MediaItem).Include(x => x.Borrower).FirstOrDefaultAsync(x => x.Id == id);

    public async Task Save(Loan loan)
    {
        if (loan.Id == 0)
        {
            _context.Loans.Add(loan);
        }
        else
        {
            var existing = await _context.Loans.FirstOrDefaultAsync(x => x.Id == loan.Id);

            if (existing == null)
                _context.Loans.Update(loan);
            else
                _context.Entry(existing).CurrentValues.SetValues(loan);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var item = await _context.Loans.FindAsync(id);
        if (item == null) return false;
        _context.Loans.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _context;
    public ReviewService(ApplicationDbContext context) => _context = context;

    public async Task<PagedResult<Review>> List(ReviewSearch search)
    {
        var query = _context.Reviews.Include(x => x.MediaItem).Include(x => x.Reviewer).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search.Keyword))
            query = query.Where(x => (x.MediaItem != null && x.MediaItem.Title.Contains(search.Keyword)) || (x.Reviewer != null && x.Reviewer.FullName.Contains(search.Keyword)) || (x.Comment != null && x.Comment.Contains(search.Keyword)));
        if (search.Rating.HasValue) query = query.Where(x => x.Rating == search.Rating.Value);
        return await query.OrderByDescending(x => x.CreatedAt).GetPagedAsync(search.Page, search.PageSize);
    }

    public async Task<Review?> Get(int id) => await _context.Reviews.Include(x => x.MediaItem).Include(x => x.Reviewer).FirstOrDefaultAsync(x => x.Id == id);

    public async Task Save(Review review)
    {
        if (review.Id == 0)
        {
            _context.Reviews.Add(review);
        }
        else
        {
            var existing = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == review.Id);

            if (existing == null)
                _context.Reviews.Update(review);
            else
                _context.Entry(existing).CurrentValues.SetValues(review);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var item = await _context.Reviews.FindAsync(id);
        if (item == null) return false;
        _context.Reviews.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
