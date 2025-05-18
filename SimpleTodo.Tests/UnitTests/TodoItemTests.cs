using System.ComponentModel.DataAnnotations;
using SimpleTodo.Api.Models;

namespace SimpleTodo.Tests.UnitTests;

public class TodoItemTests
{
    [Fact]
    public void TodoItem_TitleRequired()
    {
        TodoItem todoItem = new TodoItem() { IsCompleted = false };
        ValidationContext context = new ValidationContext(todoItem);
        List<ValidationResult> results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(todoItem, context, results, true);
        Assert.False(isValid);
        Assert.Contains(results, rs => rs.MemberNames.Contains("Title"));
    }
}