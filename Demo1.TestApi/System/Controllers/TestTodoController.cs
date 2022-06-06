using System.Threading.Tasks;
using Demo1.Api.Controllers;
using Demo1.Api.Services;
using Demo1.TestApi.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Demo1.TestApi.System.Controllers;

public class TestController
{
    [Fact]
    public async Task GetTaskAsync_ShouldReturn200Status()
    {
        ///Arrange
        var todoService = new Mock<ITodoService>();
        todoService.Setup(_ => _.GetAllAsync()).ReturnsAsync(TodoMockData.GetTodos());
        var sut = new TodoController(todoService.Object);

        ///Act
        var rs = await sut.GetAllAsync();

        ///Assert
        rs.GetType().Should().Be(typeof(OkObjectResult));
        (rs as OkObjectResult).StatusCode.Should().Be(200);
    }
    [Fact]
    public async Task GetTaskAsync_ShouldReturn204Status()
    {
        ///Arrange
        var todoService = new Mock<ITodoService>();
        todoService.Setup(_ => _.GetAllAsync()).ReturnsAsync(TodoMockData.GetEmptyTodos());
        var sut = new TodoController(todoService.Object);

        ///Act
        var rs = await sut.GetAllAsync();

        ///Assert
        rs.GetType().Should().Be(typeof(NoContentResult));
        (rs as NoContentResult).StatusCode.Should().Be(204);
    }

       [Fact]
    public async Task SaveAsync_ShouldCallTodoSaveAsyncOne()
    {
        ///Arrange
        var todoService = new Mock<ITodoService>();
        var newTodo = TodoMockData.NewTodo();
        var sut = new TodoController(todoService.Object);

        //Act
        var rs = await sut.SaveAsync(newTodo);

        ///Assert
        todoService.Verify(_ => _.SaveAsync(newTodo), Times.Exactly(1));
    }
}