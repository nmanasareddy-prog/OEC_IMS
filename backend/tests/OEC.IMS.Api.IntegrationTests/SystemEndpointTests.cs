using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace OEC.IMS.Api.IntegrationTests;

public class SystemEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SystemEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetStatus_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/v1/system/status");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_Then_GetParts_ReturnsOk()
    {
        var loginResponse = await _client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { username = "admin", password = "Admin123!" });

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();
        Assert.NotNull(login?.Token);

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/parts?page=1&pageSize=10");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", login.Token);
        var partsResponse = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, partsResponse.StatusCode);
    }

    private sealed record LoginResult(string Token, string UserId, string Username);
}
