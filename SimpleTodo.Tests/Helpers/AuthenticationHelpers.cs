using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleTodo.Tests.Helpers;

public static class AuthenticationHelpers
{
    public static async Task Authenticate(HttpClient client, string username, string password)
    {
        await Register(client, username, password);
        var response = await client.PostAsJsonAsync("/api/auth/login", new { username, password });
        response.EnsureSuccessStatusCode();
        var hasToken = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement
            .TryGetProperty("token", out JsonElement tokenElement);
        if (hasToken)
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenElement.GetString()}");
        }
    }

    public static async Task Register(HttpClient client, string username, string password)
    {
        var response = await client.PostAsJsonAsync("/api/auth/register", new { username = username, password = password});
        response.EnsureSuccessStatusCode();
    }
}