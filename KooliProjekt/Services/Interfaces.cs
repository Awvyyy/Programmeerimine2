using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services;

public interface ICategoryService
{
    Task<PagedResult<Category>> List(CategorySearch search);
    Task<IList<Category>> All();
    Task<Category?> Get(int id);
    Task Save(Category category);
    Task<bool> Delete(int id);
}

public interface IPersonService
{
    Task<PagedResult<Person>> List(PersonSearch search);
    Task<IList<Person>> All();
    Task<Person?> Get(int id);
    Task Save(Person person);
    Task<bool> Delete(int id);
}

public interface IMediaItemService
{
    Task<PagedResult<MediaItem>> List(MediaItemSearch search);
    Task<IList<MediaItem>> All();
    Task<MediaItem?> Get(int id);
    Task Save(MediaItem item);
    Task<bool> Delete(int id);
}

public interface ILoanService
{
    Task<PagedResult<Loan>> List(LoanSearch search);
    Task<Loan?> Get(int id);
    Task Save(Loan loan);
    Task<bool> Delete(int id);
}

public interface IReviewService
{
    Task<PagedResult<Review>> List(ReviewSearch search);
    Task<Review?> Get(int id);
    Task Save(Review review);
    Task<bool> Delete(int id);
}
