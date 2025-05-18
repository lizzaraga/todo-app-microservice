using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SimpleTodo.Api.Models;
using SimpleTodo.Tests.Helpers;

namespace SimpleTodo.Tests.IntegrationTests;

public class TodoControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostTodoItem_ReturnCreatedItem()
    {
        await AuthenticationHelpers.Authenticate(_client, "mael0", "mael");
        // Arrange
        var todo = new TodoItem() { Title = "Test Task", IsCompleted = false };
        var response = await _client.PostAsJsonAsync("/api/todo", todo);
        response.EnsureSuccessStatusCode();
        var todoResponse = await response.Content.ReadFromJsonAsync<TodoItem>();
        Assert.Equal(todo.Title, todoResponse?.Title);
        Assert.Equal(todo.IsCompleted, todoResponse?.IsCompleted);
    }
}