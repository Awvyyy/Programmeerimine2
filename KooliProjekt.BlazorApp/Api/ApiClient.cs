using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace KooliProjekt.BlazorApp.Api;

public interface IApiClient
{
    Task<Result<IList<MediaItem>>> List();
    Task<Result<MediaItem>> Get(int id);
    Task<Result<IList<Category>>> Categories();
    Task<Result> Save(MediaItem item);
    Task<Result> Delete(int id);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    public ApiClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<Result<IList<MediaItem>>> List()
    {
        try
        {
            var data = await _httpClient.GetFromJsonAsync<IList<MediaItem>>("MediaItemsApi");
            return Result<IList<MediaItem>>.Ok(data ?? new List<MediaItem>());
        }
        catch (Exception ex) { return Result<IList<MediaItem>>.Fail(ex.Message); }
    }

    public async Task<Result<MediaItem>> Get(int id)
    {
        try
        {
            var data = await _httpClient.GetFromJsonAsync<MediaItem>($"MediaItemsApi/{id}");
            return data == null ? Result<MediaItem>.Fail("Item not found") : Result<MediaItem>.Ok(data);
        }
        catch (Exception ex) { return Result<MediaItem>.Fail(ex.Message); }
    }

    public async Task<Result<IList<Category>>> Categories()
    {
        try
        {
            var data = await _httpClient.GetFromJsonAsync<IList<Category>>("MediaItemsApi/categories");
            return Result<IList<Category>>.Ok(data ?? new List<Category>());
        }
        catch (Exception ex) { return Result<IList<Category>>.Fail(ex.Message); }
    }

    public async Task<Result> Save(MediaItem item)
    {
        try
        {
            var response = item.Id == 0
                ? await _httpClient.PostAsJsonAsync("MediaItemsApi", item)
                : await _httpClient.PutAsJsonAsync($"MediaItemsApi/{item.Id}", item);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var result = Result.Fail("Validation failed");
                var text = await response.Content.ReadAsStringAsync();

                try
                {
                    using var doc = JsonDocument.Parse(text);
                    if (doc.RootElement.TryGetProperty("errors", out var errors))
                    {
                        foreach (var prop in errors.EnumerateObject())
                        {
                            result.Errors[prop.Name] = prop.Value.EnumerateArray()
                                .Select(x => x.GetString() ?? "Error")
                                .ToList();
                        }
                    }
                }
                catch
                {
                    // I still return the normal validation error if JSON parsing fails.
                }

                return result;
            }

            return response.IsSuccessStatusCode ? Result.Ok() : Result.Fail($"API returned {response.StatusCode}");
        }
        catch (Exception ex) { return Result.Fail(ex.Message); }
    }

    public async Task<Result> Delete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"MediaItemsApi/{id}");
            return response.IsSuccessStatusCode ? Result.Ok() : Result.Fail($"API returned {response.StatusCode}");
        }
        catch (Exception ex) { return Result.Fail(ex.Message); }
    }
}
