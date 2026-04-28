using System.Net.Http;
using System.Net.Http.Json;

namespace KooliProjekt.WpfApp.Api;

public interface IApiClient
{
    Task<Result<IList<MediaItem>>> List();
    Task<Result> Save(MediaItem item);
    Task<Result> Delete(int id);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:7136/api/") };

    public async Task<Result<IList<MediaItem>>> List()
    {
        try
        {
            var data = await _httpClient.GetFromJsonAsync<IList<MediaItem>>("MediaItemsApi");
            return Result<IList<MediaItem>>.Ok(data ?? new List<MediaItem>());
        }
        catch (Exception ex) { return Result<IList<MediaItem>>.Fail(ex.Message); }
    }

    public async Task<Result> Save(MediaItem item)
    {
        try
        {
            var response = item.Id == 0
                ? await _httpClient.PostAsJsonAsync("MediaItemsApi", item)
                : await _httpClient.PutAsJsonAsync($"MediaItemsApi/{item.Id}", item);

            return response.IsSuccessStatusCode ? Result.Ok() : Result.Fail(response.StatusCode.ToString());
        }
        catch (Exception ex) { return Result.Fail(ex.Message); }
    }

    public async Task<Result> Delete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"MediaItemsApi/{id}");
            return response.IsSuccessStatusCode ? Result.Ok() : Result.Fail(response.StatusCode.ToString());
        }
        catch (Exception ex) { return Result.Fail(ex.Message); }
    }
}
