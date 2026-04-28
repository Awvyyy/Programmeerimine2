using System.Net;
using System.Net.Http.Json;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace KooliProjekt.IntegrationTests;

[Collection("Sequential")]
public class MediaItemsApiIntegrationTests
{
    [Fact]
    public async Task Get_should_return_success()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/MediaItemsApi");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Get_by_id_should_return_not_found_for_missing_item()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/MediaItemsApi/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_should_create_item()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var item = new MediaItem { Title = "Integration item", CategoryId = 1, Price = 1, MediaType = MediaType.Book };
        var response = await client.PostAsJsonAsync("/api/MediaItemsApi", item);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
