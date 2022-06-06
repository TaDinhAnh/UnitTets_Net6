using System;
using System.Linq;
using System.Threading.Tasks;
using Demo1.Api.Data;
using Demo1.Api.Services;
using Demo1.TestApi.MockData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Demo1.TestApi.System.Services;


public class TestTodoService : IDisposable
{
    private readonly MyWorldDbContext _dbContext;

    public TestTodoService()
    {
        var opt = new DbContextOptionsBuilder<MyWorldDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _dbContext = new MyWorldDbContext(opt);
        _dbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetTaskAsync_ReturnTodoCollection()
    {
        ///Arrange
        _dbContext.Todo.AddRange(TodoMockData.GetTodos());
        _dbContext.SaveChanges();
        var sut = new TodoService(_dbContext);

        //Act
        var rs = await sut.GetAllAsync();

        ///Assert
        rs.Should().HaveCount(TodoMockData.GetTodos().Count);
    }

    [Fact]
    public async Task SaveAsync_AddNewTodo()
    {
        ///Arrange
        _dbContext.Todo.AddRange(TodoMockData.GetTodos());
        _dbContext.SaveChanges();
        var newTodo = TodoMockData.NewTodo();
        var sut = new TodoService(_dbContext);

        //Act
       await sut.SaveAsync(newTodo);

        ///Assert
       int exptedRecordCount = TodoMockData.GetTodos().Count + 1;
       _dbContext.Todo.Count().Should().Be(exptedRecordCount);
    }


    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}